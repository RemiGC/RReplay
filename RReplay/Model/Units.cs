﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.42000
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Xml.Serialization;

// 
// Ce code source a été automatiquement généré par xsd, Version=4.6.1055.0.
// 


/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
public partial class Unites {
    
    private UnitesUnit[] unitField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Unit")]
    public UnitesUnit[] Unit {
        get {
            return this.unitField;
        }
        set {
            this.unitField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class UnitesUnit {
    
    private ushort classField;
    
    private uint instanceIDField;
    
    private string classNameForDebugField;
    
    private string aliasNameField;
    
    private ushort categoryField;
    
    private ushort showRoomIDField;
    
    /// <remarks/>
    public ushort Class {
        get {
            return this.classField;
        }
        set {
            this.classField = value;
        }
    }
    
    /// <remarks/>
    public uint InstanceID {
        get {
            return this.instanceIDField;
        }
        set {
            this.instanceIDField = value;
        }
    }
    
    /// <remarks/>
    public string ClassNameForDebug {
        get {
            return this.classNameForDebugField;
        }
        set {
            this.classNameForDebugField = value;
        }
    }
    
    /// <remarks/>
    public string AliasName {
        get {
            return this.aliasNameField;
        }
        set {
            this.aliasNameField = value;
        }
    }
    
    /// <remarks/>
    public ushort Category {
        get {
            return this.categoryField;
        }
        set {
            this.categoryField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public ushort ShowRoomID {
        get {
            return this.showRoomIDField;
        }
        set {
            this.showRoomIDField = value;
        }
    }
}