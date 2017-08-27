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

Change by `ThemeManager`
```c#
    ThemeManager.CurrentTheme = ColorThemes.Classic;
```

# Change themes by ResourceDictionary

As default we use Classic theme.
You also can change theme by adding resource dictionary after generic.xaml. Use one of them:
```xml
<ResourceDictionary Source="/brightsharp;component/style.devlab.xaml" />
<ResourceDictionary Source="/brightsharp;component/style.blue.xaml" />
<ResourceDictionary Source="/brightsharp;component/style.darkblue.xaml" />
<ResourceDictionary Source="/brightsharp;component/style.silver.xaml" />
```

## NOTE
You can use custom window style named `BrightSharpWindowStyle`.
Also defined additional useful styles and extensions. For more info see Demo source code.