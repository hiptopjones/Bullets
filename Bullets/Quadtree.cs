using NLog;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bullets
{
    public class QuadtreeEntry<T>
    {
        public FloatRect BoundingBox { get; set; }
        public T Context { get; set; }
    }

    // Recursive data structure
    // Based on this article: https://thatgamesguy.co.uk/cpp-game-dev-16/
    public class Quadtree<T>
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private const int CurrentTree = -1;
        private const int NorthwestChild = 0;
        private const int NortheastChild = 1;
        private const int SouthwestChild = 2;
        private const int SoutheastChild = 3;

        public FloatRect BoundingBox { get; set; }
        public int MaxEntriesPerLevel { get; set; }

        private Quadtree<T> Parent { get; set; }
        private Quadtree<T>[] Children { get; set; } = new Quadtree<T>[4];
        private List<QuadtreeEntry<T>> Entries { get; set; } = new List<QuadtreeEntry<T>>();
        private int CurrentLevel { get; set; }

        public void Add(QuadtreeEntry<T> entry)
        {
            if (Children[0] != null) // Any children defined?
            {
                int childIndex = GetChildIndex(entry.BoundingBox);
                if (childIndex != CurrentTree)
                {
                    Children[childIndex].Add(entry);
                    return;
                }
            }

            Entries.Add(entry);

            if (Entries.Count > MaxEntriesPerLevel)
            {
                if (Children[0] == null)
                {
                    if (Split())
                    {
                        // Avoid forearch as we're mutating the list
                        // Use swap-and-pop to avoid shifting elements
                        int i = 0;
                        while (i < Entries.Count)
                        {
                            int childIndex = GetChildIndex(Entries[i].BoundingBox);
                            if (childIndex != CurrentTree)
                            {
                                Children[childIndex].Add(Entries[i]);

                                int lastIndex = Entries.Count - 1;
                                Entries[i] = Entries[lastIndex];
                                Entries.RemoveAt(lastIndex);
                            }
                            else
                            {
                                i++;
                            }
                        }
                    }
                }
            }
        }

        public void Remove(QuadtreeEntry<T> entry)
        {
            int childIndex = GetChildIndex(entry.BoundingBox);
            if (childIndex == CurrentTree)
            {
                // Avoid forearch as we're mutating the list
                // Use swap-and-pop to avoid shifting elements
                int i = 0;
                while (i < Entries.Count)
                {
                    if (Entries[i] == entry)
                    {
                        int lastIndex = Entries.Count - 1;
                        Entries[i] = Entries[lastIndex];
                        Entries.RemoveAt(lastIndex);

                        // Exit early, once found
                        break;
                    }
                    else
                    {
                        i++;
                    }
                }
            }
            else
            {
                Children[childIndex].Remove(entry);
            }
        }

        public void Clear()
        {
            Entries.Clear();

            if (Children[0] != null) // Any children defined?
            {
                for (int i = 0; i < Children.Length; i++)
                {
                    Children[i].Clear();
                    Children[i] = null;
                }
            }
        }

        // This code assumes the bounding box passed in is an object's collider,
        // and leaves checking individual candidate objects to the collision system
        // However, if the bounding box passed in is just a region of interest, we might
        // do an additional pass here and prune the candidate list further to just those
        // objects intersecting the bounding box
        public List<QuadtreeEntry<T>> Search(FloatRect boundingBox)
        {
            List<QuadtreeEntry<T>> candidateEntries = new List<QuadtreeEntry<T>>();
            candidateEntries.AddRange(Entries);

            if (Children[0] != null) // Any children defined?
            {
                int childIndex = GetChildIndex(boundingBox);
                if (childIndex == CurrentTree)
                {
                    for (int i = 0; i < Children.Length; i++)
                    {
                        if (Children[i].BoundingBox.Intersects(boundingBox))
                        {
                            candidateEntries.AddRange(Children[i].Search(boundingBox));
                        }
                    }
                }
                else
                {
                    candidateEntries.AddRange(Children[childIndex].Search(boundingBox));
                }
            }

            return candidateEntries;
        }

        private int GetChildIndex(FloatRect boundingBox)
        {
            // If the provided bounding box doesn't fit completely within a region, stay at this level
            int childIndex = CurrentTree;

            double verticalDivider = BoundingBox.Left + BoundingBox.Width / 2;
            double horizontalDivider = BoundingBox.Top + BoundingBox.Height / 2;

            bool isNorth = boundingBox.Top < horizontalDivider && boundingBox.Top + boundingBox.Height < horizontalDivider;
            bool isSouth = boundingBox.Top > horizontalDivider;
            bool isWest = boundingBox.Left < verticalDivider && boundingBox.Left + boundingBox.Width < verticalDivider;
            bool isEast = boundingBox.Left > verticalDivider;

            if (isWest)
            {
                if (isNorth)
                {
                    childIndex = NorthwestChild;
                }
                else if (isSouth)
                {
                    childIndex = SouthwestChild;
                }
            }
            else if (isEast)
            {
                if (isNorth)
                {
                    childIndex = NortheastChild;
                }
                else if (isSouth)
                {
                    childIndex = SoutheastChild;
                }
            }

            return childIndex;
        }

        private bool Split()
        {
            int childWidth = (int)(BoundingBox.Width / 2);
            int childHeight = (int)(BoundingBox.Height / 2);

            // If splitting will result in zero-sized bounds, then we can't do it
            if (childWidth == 0 || childHeight == 0)
            {
                //Debugger.Break();
                return false;
            }

            Children[NorthwestChild] = new Quadtree<T>
            {
                CurrentLevel = CurrentLevel + 1,
                Parent = this,
                MaxEntriesPerLevel = MaxEntriesPerLevel,
                BoundingBox = new FloatRect(BoundingBox.Left, BoundingBox.Top, childWidth, childHeight)
            };

            Children[NortheastChild] = new Quadtree<T>
            {
                CurrentLevel = CurrentLevel + 1,
                Parent = this,
                MaxEntriesPerLevel = MaxEntriesPerLevel,
                BoundingBox = new FloatRect(BoundingBox.Left + childWidth, BoundingBox.Top, childWidth, childHeight)
            };

            Children[SoutheastChild] = new Quadtree<T>
            {
                CurrentLevel = CurrentLevel + 1,
                Parent = this,
                MaxEntriesPerLevel = MaxEntriesPerLevel,
                BoundingBox = new FloatRect(BoundingBox.Left + childWidth, BoundingBox.Top + childHeight, childWidth, childHeight)
            };

            Children[SouthwestChild] = new Quadtree<T>
            {
                CurrentLevel = CurrentLevel + 1,
                Parent = this,
                MaxEntriesPerLevel = MaxEntriesPerLevel,
                BoundingBox = new FloatRect(BoundingBox.Left, BoundingBox.Top + childHeight, childWidth, childHeight)
            };

            return true;
        }

        public void DrawDebug()
        {
            if (Children[0] != null)
            {
                for (int i = 0; i < 4; i++)
                {
                    Children[i].DrawDebug();
                }
            }

            Debug.DrawRect(BoundingBox);
            Debug.DrawText($"L{CurrentLevel}", new Vector2f(BoundingBox.Left + BoundingBox.Width / 2, BoundingBox.Top + BoundingBox.Height / 2));

            foreach (QuadtreeEntry<T> entry in Entries)
            {
                Debug.DrawText($"L{CurrentLevel}", new Vector2f(entry.BoundingBox.Left + entry.BoundingBox.Width / 2, entry.BoundingBox.Top + entry.BoundingBox.Height / 2), Color.Blue);
            }
        }
    }
}
