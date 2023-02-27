[![Getting Started](https://img.shields.io/badge/lang-en-red.svg)](https://www.nuget.org/packages/BlazorCommon) [![Getting Started](https://img.shields.io/badge/lang-es-yellow.svg)](https://www.nuget.org/packages/BlazorCommon)

# Qué es BlazorCommon?

Es un conjunto de componentes de Blazor agrupados en una biblioteca RCL con el propósito agilizar la construcción de aplicaciones y de contribuir al conocimiento, tanto propio como colectivo.

# Dependencias

_En la versión actual_

- net7.0 (_For projects in .net 6 use Version 6.00_)
- ClosedXML (>= 0.100.3)
- Microsoft.AspNetCore.Components .Web (>= 7.0.0)

# Cómo funciona?

Tras instalar el [paquete de Nuget](https://www.nuget.org/packages/BlazorCommon) podremos utilizar los componentes Blazor.

```csharp
using BlazorCommon;
using BlazorCommon.Grid;
...

```

## Grid

El grid es uno de los componentes más usados para mostrar información de una manera práctica. Es una parte del trabajo común a la mayoría de los desarrolladores. Es también un problema a la hora de mantener el código y el rendimiento de los proyectos. presentamos una opción simple y configurable.

![Simple grid](./GridExample.jpg)

##### Cómo implementarlo

_Code behind_

```csharp
using BlazorCommon.Grid;
using BlazorCommon;
using Microsoft.AspNetCore.Components;

namespace BlazorCommonTester.Pages
{
    public class GridBase : HtmlComponentBase
    {
        protected GridConfigurationBase GridC { get; set; }
        protected override async Task OnInitializedAsync()
        {
            GridC = await GridConfigurationBase.GetInstanceAsync();
           await base.OnInitializedAsync();
        }

    }
}

```

_Front_

```c#
<BlazorCommon.Grid.Grid GridConfig="GridC"  />
```

Si su interés prácitco va más allá de presentar la misma información que ponemos por defecto o no está interesado en la última vez que fue avistado algún animal, todo lo que tiene que hacer es sobrescribir el método GetList() de la clase QueryResultBase.
ryResultBase Class.

_QueryResultBase Class_

```csharp
 public class QueryResultBase
    {
        public int NotFilteredTotal { get; set; }
        public int Total { get; set; }
        public IEnumerable<RowBase> List { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public SortChangedEvent Sort { get; set; }

        public QueryResultBase()
        {
            PageSize = 10;
            PageIndex = 1;
        }

        public virtual void GetList()
        {
            List = Animal.GetAll();
        }
```

**Note:** La propiedad **List** de la clase deriva de otra llamada RowBase; así, para que todo funcione _los objetos que conformen la lista a presentar en el grid deben ser de tipo RowBase o de una clase derivada._

```csharp
 public IEnumerable<RowBase> List { get; set; } // Property of QueryResultBase
```

```csharp
   public class RowBase
    {
        public string RowBaseBackGroundColor { get; set; }
        public string RowBaseFontColor { get; set; }
        public string RowBaseId { get; set; }
        public bool RowBaseSelected { get; set; }
        public bool RowBasePreviousSelected { get; set; }
        public bool RowBaseVisible { get; set; }
        public List<ExpandedRowOption> ExpandedRowOptions { get; set; }
        public bool RowExpanded { get; set; }

        public RowBase()
        {
            RowBaseBackGroundColor = "white";
            RowBaseFontColor = "black";
            Random r = new Random();
            int rInt = r.Next(0, 100000000);
            RowBaseId = $"row_{rInt}";
            RowBaseVisible= true;
        }

        public virtual void SetExpandedRowOptions()
        {
            ExpandedRowOptions = Enum.GetValues(typeof(ExpandedRowOption)).Cast<ExpandedRowOption>().ToList();
        }

        public virtual async Task OnRowClick(JsHelper jsHelper)
        {
            RowExpanded = !RowExpanded;
            if(RowExpanded)
            {
                SetExpandedRowOptions();
                await jsHelper.SetSessionStorage("row", this);
            }
            else
            {
                await jsHelper.RemoveSessionStorage("row");
            }
        }

    }
```

La clase RowBase determina el comportamiento y la apariencia de las filas dentro de la tabla.

```csharp
 public class MyList: RowBase
    {
       ...
    }
```

##### Filtrado y ordenación

Por defecto el grid implementa filtros y ordenación para todas sus columnas. Esto puede cambiarse en los parámetros que recibe el constructor de la clase GridColumnase. Ésta, a su vez, es inicializada en GridConfigurationBase, donde se puede sobreescribir el método que construye la anterior. Mucho lío, ¿no?. Mejor con un ejemplo.

![Getting filter grid](./filter_example.jpg)

```csharp

  public virtual List<GridColumnBase> GetGridColumnBase()
        {
            List<PropertyInfo> baseProperties = new RowBase().GetType().GetProperties().ToList();
            List<PropertyInfo> props = ItemType.GetProperties().Where(x => !baseProperties.Any(s => s.Name == x.Name)).ToList();

           return props.Select(x => new GridColumnBase(x, props.IndexOf(x), KeyColumn)).ToList();
        }

// Este es el método que construye las columnas y se puede sobreescribir para cambiar los parámetros que recibe el constructor.

 public GridColumnBase(PropertyInfo prop, int position, string keyColumnName, bool searchable =true, bool sortable= true)
        {
            ...
        }
// Constructor de la clase GridColumnBase: searchable y sortable determinan si la columna en cuestión se podrá filtrar, en el primer caso o ordenar en el segundo.

```

## Conclusiones

Sin mucho esfuerzo se puede cambiar el origen de datos del grid, pero hay muchas otras configuraciones que son imporantes, como el aspecto o los enlaces que se pueden añadir. Invitamos a que explore el código en el proyecto que podrá encontrar en [GitHub.](https://github.com/alexarriete/BlazorCommon)
