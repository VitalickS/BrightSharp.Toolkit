### ZoomControl usage example

`DraggableExtender` for `ZoomControl`. Example:
```xaml
<bsCtrl:ZoomControl xmlns:bsCtrl="http://schemas.brightsharp.com/controls"
                    xmlns:bs="http://schemas.brightsharp.com/developer">
    <Canvas>
        <Ellipse bs:DraggableExtender.CanDrag="true" Canvas.Left="37" Canvas.Top="110" Width="100" Height="100" Fill="Blue" />
        <Rectangle bs:DraggableExtender.CanDrag="true" Canvas.Left="193" Canvas.Top="55" Width="100" Height="100" Fill="Black" />
    </Canvas>
</bsCtrl:ZoomControl>
```