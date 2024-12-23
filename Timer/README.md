This an engine/framework agnostic library meant to be used to create timers for games. These timers support delays, loops, pausing and events.

Basic features for updating
```cs
GameTimer timer = new(5f);

timer.Update(5f);

Console.WriteLine(timer.IsFinished);
```


### Looping

```cs
string text = "Hello, how are you doing today?";

GameTimer timer = new(0.2f, loopCount: text.Length, () => {
    Console.WriteLine(text.Substring(0, timer.Count));
});
```


### Delays
