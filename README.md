# Entity Component System 

## Usage

Below is an example of using the ECS service in the context of a video game. It defines a render system that looks to render entities that have a transform and image component.

```c#
class Game {
    private ECS.Service ecs = new ECS.Service();
    private RenderSystem renderSystem;
    
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

class RenderSystem {
    private object[] components = new object[2];
    private ECS.Service ecs;

    public RenderService(ECS.Service ecs) {
        this.ecs = ecs;
    }

    public void Tick() {
        // Iterate over all entities that have both a transform and image
        // component.
        Cursor cursor = ecs.Query(typeof(Transform), typeof(Image));

        while (cursor.Next(components)) {
            Transform transform = components[0] as Transform;
            Image image = components[1] as Image;

            Graphics.Draw(image.bitmap, transform.x, tranform.y);
        }
    }
}
```
