using SnakeGame.Mechanics.Collision.Shapes;
using System.Numerics;

namespace SnakeGame.Mechanics.Collision.Resolvers;

internal class PolygonToPolygonResolver : ICollisionResolver<Polygon, Polygon>
{
    public bool IsColliding(Polygon body1, Polygon body2)
    {
        Vector2 perpendicularLine;
        float dot;
        var perpendicularStack = new List<Vector2>();
        float? amin, amax, bmin, bmax;

        // Work out all perpendicular vectors on each edge for polygonA
        foreach (var edge in body1.Edges)
        {
            perpendicularLine = new Vector2(-edge.Y, edge.X);
            perpendicularStack.Add(perpendicularLine);
        }

        // Work out all perpendicular vectors on each edge for polygonB
        foreach (var edge in body2.Edges)
        {
            perpendicularLine = new Vector2(-edge.Y, edge.X);
            perpendicularStack.Add(perpendicularLine);
        }

        // Loop through each perpendicular vector for both polygons
        foreach (var perpendicularVector in perpendicularStack)
        {
            amin = null;
            amax = null;
            bmin = null;
            bmax = null;

            // Work out all of the dot products for all of the vertices in PolygonA against the perpendicular vector
            foreach (var vertex in body1.Vertexes)
            {
                dot = Vector2.Dot(vertex, perpendicularVector);
                if (amax == null || dot > amax)
                    amax = dot;
                if (amin == null || dot < amin)
                    amin = dot;
            }

            // Work out all of the dot products for all of the vertices in PolygonB against the perpendicular vector
            foreach (var vertex in body2.Vertexes)
            {
                dot = Vector2.Dot(vertex, perpendicularVector);
                if (bmax == null || dot > bmax)
                    bmax = dot;
                if (bmin == null || dot < bmin)
                    bmin = dot;
            }

            // If there is no gap between the dot products projection then we will continue onto evaluating the next perpendicular edge
            if ((amin < bmax && amin > bmin) || (bmin < amax && bmin > amin))
                continue;

            // Otherwise, we know that there is no collision for definite
            else
                return false;
        }

        // If we have gotten this far, where we have looped through all of the perpendicular edges and not a single one of their projections had a gap in them
        // Then we know that the 2 polygons are colliding for definite
        return true;
    }
}
