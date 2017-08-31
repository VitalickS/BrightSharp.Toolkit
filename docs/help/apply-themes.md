# Apply themes with App.xaml 

`App.xaml`
```xml
<ResourceDictionary>
    <ResourceDictionary.MergedDictionaries>
        
        <ResourceDictionary Source="/brightsharp;component/themes/style.classic.xaml" />
        <ResourceDictionary Source="/brightsharp;component/themes/theme.xaml" />
        
    </ResourceDictionary.MergedDictionaries>

</ResourceDictionary>
```

# Change themes by ThemeManager

Change by `ThemeManager`
```c#
    ThemeManager.CurrentTheme = ColorThemes.DarkBlue;
```

# Change themes by ResourceDictionary

As default we use Classic theme.
You also can change theme by adding resource dictionary after generic.xaml. Use one of them:
```xml
<ResourceDictionary Source="/brightsharp;component/themes/style.classic.xaml" />
<ResourceDictionary Source="/brightsharp;component/themes/style.devlab.xaml" />
<ResourceDictionary Source="/brightsharp;component/themes/style.silver.xaml" />
<ResourceDictionary Source="/brightsharp;component/themes/style.blue.xaml" />
<ResourceDictionary Source="/brightsharp;component/themes/style.darkblue.xaml" />
```

## NOTES
You can use custom window style named `BrightSharpWindowStyle`.
Also defined additional useful styles and extensions. For more info see Demo source code.

<!--Theme resource use `DynamicResource`. If you want, you can use `StaticResource` via **`theme.static.xaml`**.-->