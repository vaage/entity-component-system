# Entity Component System 

## Usage

Below is an example of using the ECS service in the context of a video game. It defines a render system that looks to render entities that have a transform and image component.

```c#
class Game {
    private ECS.Service ecs = new ECS.Service();
    private AbstractSystem renderSystem;
    
    public Game() {
        renderSystem = new RenderSystem(ecs);
    }
    
    public void Load() {
        // Create a new entity and add some components to it.
        Entity entity = service.CreateEntity();

        Transform tranform = service.CreateComponent<Transform>(entity);
        transform.x = 0;
        transform.y = 0;

        Image image = service.CreateComponent<Image>(entity);
        image.bitmap = Graphics.Load("my-image.png");
    }
    
    public void Draw() {
        renderSystem.Tick();
    }
}

class RenderSystem : AbstractSystem {
    private object[] components = new object[2];

    public void Tick() {
        // Iterate over all entities that have both a transform and image
        // component.
        Cursor cursor = Query(typeof(Transform), typeof(Image));

        while (cursor.Next(components)) {
            var transform = components[0] as Transform;
            var image = components[1] as Image;

            Graphics.Draw(image.bitmap, transform.x, tranform.y);
        }
    }
}
```
