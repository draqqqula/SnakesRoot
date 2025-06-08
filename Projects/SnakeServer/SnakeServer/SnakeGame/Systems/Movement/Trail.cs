using SnakeCore.MathExtensions;

namespace SnakeGame.Systems.Movement;

internal class Trail
{
    public float TotalLength { get; private set; }

    public TrailNode? Head { get; private set; }
    public TrailNode? Tail { get; private set; }

    public void Clear()
    {
        TotalLength = 0;
        Head = null;
        Tail = null;
    }

    public void ExtendBack(TrailNode item)
    {
        var last = item.Last();
        var first = item.First();
        TotalLength += GetLength(last);
        if (Head is null || Tail is null)
        {
            Tail = last;
            Head = first;
        }
        else if (last.Rotation == Head.Rotation)
        {
            Tail.Previous = first.Previous;
            if (first.Previous is not null)
            {
                first.Previous.Next = Tail;
            }
            Tail.DistanceTraveled += first.DistanceTraveled;
            Tail = Tail.Last();
        }
        else
        {
            first.Next = Tail;
            Tail.Previous = first;
            Tail = last;
        }
    }

    public void ExtendFront(TrailNode item)
    {
        var last = item.Last();
        var first = item.First();
        TotalLength += GetLength(last);
        if (Head is null || Tail is null)
        {
            Tail = last;
            Head = first;
        }
        else if (last.Rotation == Head.Rotation)
        {
            Head.Next = last.Next;
            if (last.Next is not null)
            {
                last.Next.Previous = Head;
            }
            Head.Position = last.Position;
            Head.DistanceTraveled += last.DistanceTraveled;
            Head = Head.First();
        }
        else 
        {
            last.Previous = Head;
            Head.Next = last;
            Head = first;
        }
    }

    public TrailNode? Cut(float distance)
    {
        var root = Tail;
        var current = Tail;
        var remaining = distance;
        while (remaining > 0 && current is not null)
        {
            if (current.DistanceTraveled < remaining)
            {
                remaining -= current.DistanceTraveled;
                current = current.Next;
            }
            else if (current.DistanceTraveled == remaining)
            {
                if (current.Next is not null)
                {
                    current.Next.Previous = null;
                }
                Tail = current.Next;
                current.Next = null;
                TotalLength -= distance;
                return root;
            }
            else
            {
                Tail = current;
                TotalLength -= distance;
                return Split(current, remaining);
            }
        }
        TotalLength = 0;
        Head = null;
        Tail = null;
        return root;
    }

    private static float GetLength(TrailNode end)
    {
        var current = end;
        var counter = 0f;
        while (current is not null)
        {
            counter += current.DistanceTraveled;
            current = current.Next;
        }
        return counter;
    }

    private static TrailNode Split(TrailNode item, float distance)
    {
        var direction = MathEx.AngleToVector(item.Rotation);

        var rest = item with
        {
            DistanceTraveled = distance,
            Position = item.Position - direction * (item.DistanceTraveled - distance),
            Next = null
        };
        item.DistanceTraveled -= distance;

        if (item.Previous is not null)
        {
            item.Previous.Next = rest;
        }
        item.Previous = null;

        return rest;
    }
}
