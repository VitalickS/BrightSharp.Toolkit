# Apply themes with App.xaml 

`App.xaml`
```xml
<ResourceDictionary>
    <ResourceDictionary.MergedDictionaries>
        <ResourceDictionary Source="/brightsharp;component/generic.xaml" />
        
    </ResourceDictionary.MergedDictionaries>

</ResourceDictionary>
```

# Apply themes by ThemeManager

Use enumeration at singleton
```c#
    ThemeManager.CurrentTheme = ColorThemes.Classic;
```

## NOTE
It is recommended use `Background="{DynamicResource WindowBackgroundBrush}"` for Window markup
