using Microsoft.Extensions.DependencyInjection;

namespace SnakeGame.Mechanics.Collision;

internal interface ICollisionChecker
{
    public bool IsColliding<T1, T2>(IBodyComponent<T1> bodyA, IBodyComponent<T2> bodyB) 
        where T1 : IFlatShape 
        where T2 : IFlatShape;
}

internal class CollisionChecker(IServiceProvider Services) : ICollisionChecker
{
    private object? CachedResolver;
    public bool IsColliding<T1, T2>(IBodyComponent<T1> bodyA, IBodyComponent<T2> bodyB) where T1 : IFlatShape
        where T2 : IFlatShape
    {
        {
            if (CachedResolver is ICollisionResolver<T1, T2> resolver)
            {
                return CheckAny(bodyA.GetBody(), bodyB.GetBody(), resolver);
            }
        }
        {
            var resolver = Services.GetService<ICollisionResolver<T1, T2>>();
            if (resolver is null)
            {
                return false;
            }
            CachedResolver = resolver;
            return CheckAny(bodyA.GetBody(), bodyB.GetBody(), resolver);
        }
    }

    public static bool CheckAny<T1, T2>(IEnumerable<T1> bodyGroupA, IEnumerable<T2> bodyGroupB, ICollisionResolver<T1, T2> resolver)
    {
        foreach (var bodyA in bodyGroupA)
        {
            foreach (var bodyB in bodyGroupB)
            {
                if (resolver.IsColliding(bodyA, bodyB))
                {
                    return true;
                }
            }
        }
        return false;
    }
}