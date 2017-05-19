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

# Change themes by ResourceDictionary

Also you can change theme by adding resource dictionary after generic.xaml. Use one of them:
```xml
<ResourceDictionary Source="/brightsharp;component/style.devlab.xaml" />
or
<ResourceDictionary Source="/brightsharp;component/style.blue.xaml" />
or
<ResourceDictionary Source="/brightsharp;component/style.darkblue.xaml" />
or
<ResourceDictionary Source="/brightsharp;component/style.silver.xaml" />
```

## NOTE
It is recommended use `Background="{DynamicResource WindowBackgroundBrush}"` for Window markup
