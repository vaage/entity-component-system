namespace ECS
{
    public struct Entity
    {
        public int Service { get; }
        public int Id { get; }

        public Entity(int service, int id)
        {
            Service = service;
            Id = id;
        }
    }
}