using System;
using System.Collections.Generic;

namespace ECS
{
    using ComponentGroup = SortedList<int, object>;

    public sealed class Cursor
    {
        private readonly IEnumerator<KeyValuePair<int, object>>[] iterators;

        public Cursor(params ComponentGroup[] groups)
        {
            iterators = new IEnumerator<KeyValuePair<int, object>>[groups.Length];

            for (int i = 0; i < groups.Length; i++)
            {
                var iterator = groups[i].GetEnumerator();

                if (iterator.MoveNext())
                {
                    iterators[i] = iterator;
                }
            }
        }

        public bool Next(object[] values)
        {
            // Make sure we have something to iterator over and make sure we
            // have enough room to store the results.
            if (iterators.Length == 0 || iterators.Length > values.Length)
            {
                return false;
            }

            // All termination cases are inside the loop.
            while (true)
            {
                // Check if we reached the end of any group.
                for (int i = 0; i < iterators.Length; i++)
                {
                    if (iterators[i] == null)
                    {
                        return false;
                    }
                }

                // Find the min and max entity values. If they are the same, it
                // means we found the components for a single entity.
                int min = iterators[0].Current.Key;
                int max = iterators[0].Current.Key;

                for (int i = 0; i < iterators.Length; i++)
                {
                    min = Math.Min(min, iterators[i].Current.Key);
                    max = Math.Max(max, iterators[i].Current.Key);
                }

                // If the min and the max are the same, it means that all values
                // match. Make sure to use the iterator's length since the
                // values; length can be larger than the iterators' length.
                if (min == max)
                {
                    for (int i = 0; i < iterators.Length; i++)
                    {
                        values[i] = iterators[i].Current.Value;
                    }

                    // Because we found a match, we need to step forward one so
                    // that next time we get called we won't be on this entity again.
                    Advance(min);

                    return true;
                }

                // Right now we are pointing to different entities, we need to
                // move forward one step.
                Advance(min);
            }
        }

        private void Advance(int min)
        {
            // Check which iterators we need to move forward. If we reach the
            // end of an iterator, remove it by setting it to null so that we
            // will know we are done (no more possible matches).
            for (int i = 0; i < iterators.Length; i++)
            {
                var iterator = iterators[i];

                if (iterator.Current.Key == min && !iterator.MoveNext())
                {
                    iterators[i] = null;
                }
            }
        }
    }
}