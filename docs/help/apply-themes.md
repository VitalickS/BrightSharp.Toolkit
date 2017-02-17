# Apply themes with App.xaml 

`App.xaml`
```xml
<ResourceDictionary>
    <ResourceDictionary.MergedDictionaries>
        <ResourceDictionary Source="/brightsharp;component/generic.xaml" />
        
    </ResourceDictionary.MergedDictionaries>

</ResourceDictionary>
```

# Change themes by ThemeManager

Change by static property
```c#
    ThemeManager.CurrentTheme = ColorThemes.Classic;
```

## NOTE
It is recommended use `Background="{DynamicResource WindowBackgroundBrush}"` for Window markup
