# Documentation for Development

## Table Of Contents

- [DebugMessages](#debugmessages)

## DebugMessages

Add a `DebugMessage` object to your class, and wire it to the global `DEBUG_MESSAGE` variable.

To add a message to it, use the `DebugMessage`.`Msg()` method.

Example:

```c#
public class MyClass {
    private readonly DebugMessage DEBUG;

    public MyClass(DebugMessage DEBUG){
        this.DEBUG = DEBUG;
    }

    public void MyMethod(){
        DEBUG.Msg("Debug Text");
    }
}
```
