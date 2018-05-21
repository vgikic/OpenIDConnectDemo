${
    using Typewriter.Extensions.Types;    
    Template(Settings settings)
    {
       // settings.IncludeCurrentProject().IncludeProject("WebTesting.Service");        
    } 

	Type CalculatedType(Type t)
    {
        var type = t;
        while (!type.IsEnumerable && type.IsGeneric) {
            type = type.Unwrap();
        }
        return type;
    }

    Class GetBaseClassIfExists(Class c) {return c.BaseClass ?? null;}  
     
    Type[] CalculatedModelTypes(Class c)
    {
        // get base class name if it exists
        string baseClassName = c.BaseClass != null ? c.BaseClass.Name : "";
        var allTypes = c.Properties
            .Select(m => CalculatedType(m.Type))            
            .Where(t => t != null && t.Name != baseClassName && (t.IsDefined || (t.IsEnumerable && !t.IsPrimitive))) // avoid importing base class (it will be imported with calculated base)
            .ToLookup(t => t.ClassName(), t => t);

        return allTypes                    
                    .ToDictionary(l => l.Key, l => l.First())
                    .Select(kvp => kvp.Value)
                    .Where(kvp => kvp.Name != "T[]" && kvp.Name != c.Name && kvp.Name != c.Name + "[]") // prevention of importing generic
                    .ToArray();
    } 
    string CalculatedTypeName(Type t)
    {
        var type = CalculatedType(t);
        return type != null ? type.Name : "void";
    }

    string TypeMap(Property property)
    {
        var type = property.Type;

        if (type.IsPrimitive)
        {
            return type.IsDate ?
                $"new Date(data.{UpperCasePropertyName(property)})" :
                $"data.{UpperCasePropertyName(property)}";
        }
        else
        {
            return type.IsEnumerable ?
                $"data.{UpperCasePropertyName(property)}.map(i => new {type.Name.TrimEnd('[', ']')}(i))" :
                $"new {type.Name}(data.{UpperCasePropertyName(property)})";
        }
    }

    string UpperCasePropertyName(Property property)
    {
       return property.Name[0].ToString().ToUpperInvariant() + property.Name.Substring(1);
    }
    
    string LowerCasePropertyName(Property property)
    {
       return property.Name[0].ToString().ToLowerInvariant() + property.Name.Substring(1);
    }

    string GetPropertiesAsArguments(Class c)
    {
        var result = "";
        for(int i = 0; i< c.Properties.Count; i++)
        {
            var separator = (i == c.Properties.Count -1) ? "" : ", ";
            var prop = c.Properties[i];
            result += $"{LowerCasePropertyName(prop)} : {prop.Type} = null{separator} ";
        }
        return result;
    }

}$Classes(f=> (f.Namespace.EndsWith("Client.SPA.DTO") || f.Namespace.EndsWith("BindingModels")) && !f.Attributes.Select(a=> a.Name.ToLower()).Contains("typewriterignore"))[$CalculatedModelTypes[import {$ClassName} from './$ClassName';
]$GetBaseClassIfExists[import {I$Name, $Name} from './$Name';
]
//----------------------------------------------
//  DTO interface and model
//----------------------------------------------

export interface I$Name$TypeParameters $GetBaseClassIfExists[extends I]$GetBaseClassIfExists {$Properties[
    $UpperCasePropertyName: $Type;]
}

export class $Name$TypeParameters $GetBaseClassIfExists[extends ]$GetBaseClassIfExists implements I$Name$TypeParameters{ $Properties[
    public $UpperCasePropertyName: $Type;]

    constructor($GetPropertiesAsArguments) {
        $GetBaseClassIfExists[super();]$Properties[
        this.$UpperCasePropertyName = $LowerCasePropertyName;]
    }
}]

$Enums(McD.MailJet.Portal.Models.*)[export enum $Name { 
          $Values[
          $Name = $Value][,]
}]
