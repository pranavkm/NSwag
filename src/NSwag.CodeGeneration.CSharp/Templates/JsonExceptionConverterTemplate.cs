﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 15.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace NSwag.CodeGeneration.CSharp.Templates
{
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "C:\Projects\NSwag\src\NSwag.CodeGeneration.CSharp\Templates\JsonExceptionConverterTemplate.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "15.0.0.0")]
    internal partial class JsonExceptionConverterTemplate : JsonExceptionConverterTemplateBase
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public virtual string TransformText()
        {
            this.Write("[System.CodeDom.Compiler.GeneratedCode(\"NSwag\", \"");
            
            #line 2 "C:\Projects\NSwag\src\NSwag.CodeGeneration.CSharp\Templates\JsonExceptionConverterTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(SwaggerDocument.ToolchainVersion));
            
            #line default
            #line hidden
            this.Write(@""")]
internal class JsonExceptionConverter : Newtonsoft.Json.JsonConverter
{
	private readonly Newtonsoft.Json.Serialization.DefaultContractResolver _defaultContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
	private readonly System.Collections.Generic.IDictionary<string, System.Reflection.Assembly> _searchedNamespaces;
	private readonly bool _hideStackTrace = false;
	
	public JsonExceptionConverter()
	{
        _searchedNamespaces = new System.Collections.Generic.Dictionary<string, System.Reflection.Assembly> { { typeof(");
            
            #line 11 "C:\Projects\NSwag\src\NSwag.CodeGeneration.CSharp\Templates\JsonExceptionConverterTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ExceptionModelClass));
            
            #line default
            #line hidden
            this.Write(").Namespace, System.Reflection.IntrospectionExtensions.GetTypeInfo(typeof(");
            
            #line 11 "C:\Projects\NSwag\src\NSwag.CodeGeneration.CSharp\Templates\JsonExceptionConverterTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ExceptionModelClass));
            
            #line default
            #line hidden
            this.Write(")).Assembly } };\r\n\t}\r\n\t\r\n\tpublic override bool CanWrite => true;\r\n\t\r\n\tpublic over" +
                    "ride void WriteJson(Newtonsoft.Json.JsonWriter writer, object value, Newtonsoft." +
                    "Json.JsonSerializer serializer)\r\n\t{\r\n\t\tvar exception = value as System.Exception" +
                    ";\r\n\t\tif (exception != null)\r\n\t\t{\r\n\t\t\tvar resolver = serializer.ContractResolver " +
                    "as Newtonsoft.Json.Serialization.DefaultContractResolver ?? _defaultContractReso" +
                    "lver;\r\n\t\r\n\t\t\tvar jObject = new Newtonsoft.Json.Linq.JObject();\r\n\t\t\tjObject.Add(r" +
                    "esolver.GetResolvedPropertyName(\"discriminator\"), exception.GetType().Name);\r\n\t\t" +
                    "\tjObject.Add(resolver.GetResolvedPropertyName(\"Message\"), exception.Message);\r\n\t" +
                    "\t\tjObject.Add(resolver.GetResolvedPropertyName(\"StackTrace\"), _hideStackTrace ? " +
                    "\"HIDDEN\" : exception.StackTrace);\r\n\t\t\tjObject.Add(resolver.GetResolvedPropertyNa" +
                    "me(\"Source\"), exception.Source);\r\n\t\t\tjObject.Add(resolver.GetResolvedPropertyNam" +
                    "e(\"InnerException\"),\r\n\t\t\t\texception.InnerException != null ? Newtonsoft.Json.Lin" +
                    "q.JToken.FromObject(exception.InnerException, serializer) : null);\r\n\t\r\n\t\t\tforeac" +
                    "h (var property in GetExceptionProperties(value.GetType()))\r\n\t\t\t{\r\n\t\t\t\tvar prope" +
                    "rtyValue = property.Key.GetValue(exception);\r\n\t\t\t\tif (propertyValue != null)\r\n\t\t" +
                    "\t\t{\r\n\t\t\t\t\tjObject.AddFirst(new Newtonsoft.Json.Linq.JProperty(resolver.GetResolv" +
                    "edPropertyName(property.Value),\r\n\t\t\t\t\t\tNewtonsoft.Json.Linq.JToken.FromObject(pr" +
                    "opertyValue, serializer)));\r\n\t\t\t\t}\r\n\t\t\t}\r\n\t\r\n\t\t\tvalue = jObject;\r\n\t\t}\r\n\t\r\n\t\tseri" +
                    "alizer.Serialize(writer, value);\r\n\t}\r\n\t\r\n\tpublic override bool CanConvert(System" +
                    ".Type objectType)\r\n\t{\r\n\t\treturn System.Reflection.IntrospectionExtensions.GetTyp" +
                    "eInfo(typeof(System.Exception)).IsAssignableFrom(System.Reflection.Introspection" +
                    "Extensions.GetTypeInfo(objectType));\r\n\t}\r\n\t\r\n\tpublic override object ReadJson(Ne" +
                    "wtonsoft.Json.JsonReader reader, System.Type objectType, object existingValue, N" +
                    "ewtonsoft.Json.JsonSerializer serializer)\r\n\t{\r\n\t\tvar jObject = serializer.Deseri" +
                    "alize<Newtonsoft.Json.Linq.JObject>(reader);\r\n\t\tif (jObject == null)\r\n\t\t\treturn " +
                    "null;\r\n\t\r\n\t\tvar newSerializer = new Newtonsoft.Json.JsonSerializer();\r\n\t\tnewSeri" +
                    "alizer.ContractResolver = (Newtonsoft.Json.Serialization.IContractResolver)Syste" +
                    "m.Activator.CreateInstance(serializer.ContractResolver.GetType());\r\n\t\r\n\t\tvar fie" +
                    "ld = GetField(typeof(Newtonsoft.Json.Serialization.DefaultContractResolver), \"_s" +
                    "haredCache\");\r\n\t\tif (field != null)\r\n\t\t\tfield.SetValue(newSerializer.ContractRes" +
                    "olver, false);\r\n\t\r\n\t\tdynamic resolver = newSerializer.ContractResolver;\r\n\t\tif (S" +
                    "ystem.Reflection.RuntimeReflectionExtensions.GetRuntimeProperty(newSerializer.Co" +
                    "ntractResolver.GetType(), \"IgnoreSerializableAttribute\") != null)\r\n\t\t\tresolver.I" +
                    "gnoreSerializableAttribute = true;\r\n\t\tif (System.Reflection.RuntimeReflectionExt" +
                    "ensions.GetRuntimeProperty(newSerializer.ContractResolver.GetType(), \"IgnoreSeri" +
                    "alizableInterface\") != null)\r\n\t\t\tresolver.IgnoreSerializableInterface = true;\r\n\t" +
                    "\r\n\t\tNewtonsoft.Json.Linq.JToken token;\r\n\t\tif (jObject.TryGetValue(\"discriminator" +
                    "\", System.StringComparison.OrdinalIgnoreCase, out token))\r\n\t\t{\r\n\t\t\tvar discrimin" +
                    "ator = Newtonsoft.Json.Linq.Extensions.Value<string>(token);\r\n\t\t\tif (objectType." +
                    "Name.Equals(discriminator) == false)\r\n\t\t\t{\r\n\t\t\t\tvar exceptionType = System.Type." +
                    "GetType(\"System.\" + discriminator, false);\r\n\t\t\t\tif (exceptionType != null)\r\n\t\t\t\t" +
                    "\tobjectType = exceptionType;\r\n\t\t\t\telse\r\n\t\t\t\t{\r\n\t\t\t\t\tforeach (var pair in _search" +
                    "edNamespaces)\r\n\t\t\t\t\t{\r\n\t\t\t\t\t\texceptionType = pair.Value.GetType(pair.Key + \".\" +" +
                    " discriminator);\r\n\t\t\t\t\t\tif (exceptionType != null)\r\n\t\t\t\t\t\t{\r\n\t\t\t\t\t\t\tobjectType =" +
                    " exceptionType;\r\n\t\t\t\t\t\t\tbreak;\r\n\t\t\t\t\t\t}\r\n\t\t\t\t\t}\r\n\t\r\n\t\t\t\t}\r\n\t\t\t}\r\n\t\t}\r\n\t\r\n\t\tvar v" +
                    "alue = jObject.ToObject(objectType, newSerializer);\r\n\t\tforeach (var property in " +
                    "GetExceptionProperties(value.GetType()))\r\n\t\t{\r\n\t\t\tvar jValue = jObject.GetValue(" +
                    "resolver.GetResolvedPropertyName(property.Value));\r\n\t\t\tvar propertyValue = (obje" +
                    "ct)jValue?.ToObject(property.Key.PropertyType);\r\n\t\t\tif (property.Key.SetMethod !" +
                    "= null)\r\n\t\t\t\tproperty.Key.SetValue(value, propertyValue);\r\n\t\t\telse\r\n\t\t\t{\r\n\t\t\t\tfi" +
                    "eld = GetField(objectType, \"m_\" + property.Value.Substring(0, 1).ToLowerInvarian" +
                    "t() + property.Value.Substring(1));\r\n\t\t\t\tif (field != null)\r\n\t\t\t\t\tfield.SetValue" +
                    "(value, propertyValue);\r\n\t\t\t}\r\n\t\t}\r\n\t\r\n\t\tSetExceptionFieldValue(jObject, \"Messag" +
                    "e\", value, \"_message\", resolver, newSerializer);\r\n\t\tSetExceptionFieldValue(jObje" +
                    "ct, \"StackTrace\", value, \"_stackTraceString\", resolver, newSerializer);\r\n\t\tSetEx" +
                    "ceptionFieldValue(jObject, \"Source\", value, \"_source\", resolver, newSerializer);" +
                    "\r\n\t\tSetExceptionFieldValue(jObject, \"InnerException\", value, \"_innerException\", " +
                    "resolver, serializer);\r\n\t\r\n\t\treturn value;\r\n\t}\r\n\t\r\n\tprivate System.Reflection.Fi" +
                    "eldInfo GetField(System.Type type, string fieldName)\r\n\t{\r\n\t\tvar field = System.R" +
                    "eflection.IntrospectionExtensions.GetTypeInfo(type).GetDeclaredField(fieldName);" +
                    "\r\n\t\tif (field == null && System.Reflection.IntrospectionExtensions.GetTypeInfo(t" +
                    "ype).BaseType != null)\r\n\t\t\treturn GetField(System.Reflection.IntrospectionExtens" +
                    "ions.GetTypeInfo(type).BaseType, fieldName);\r\n\t\treturn field;\r\n\t}\r\n\t\r\n\tprivate S" +
                    "ystem.Collections.Generic.IDictionary<System.Reflection.PropertyInfo, string> Ge" +
                    "tExceptionProperties(System.Type exceptionType)\r\n\t{\r\n\t\tvar result = new System.C" +
                    "ollections.Generic.Dictionary<System.Reflection.PropertyInfo, string>();\r\n\t\tfore" +
                    "ach (var property in System.Linq.Enumerable.Where(System.Reflection.RuntimeRefle" +
                    "ctionExtensions.GetRuntimeProperties(exceptionType), \r\n\t\t\tp => p.GetMethod?.IsPu" +
                    "blic == true))\r\n\t\t{\r\n\t\t\tvar attribute = System.Reflection.CustomAttributeExtensi" +
                    "ons.GetCustomAttribute<Newtonsoft.Json.JsonPropertyAttribute>(property);\r\n\t\t\tvar" +
                    " propertyName = attribute != null ? attribute.PropertyName : property.Name;\r\n\t\r\n" +
                    "\t\t\tif (!System.Linq.Enumerable.Contains(new[] { \"Message\", \"StackTrace\", \"Source" +
                    "\", \"InnerException\", \"Data\", \"TargetSite\", \"HelpLink\", \"HResult\" }, propertyName" +
                    "))\r\n\t\t\t\tresult[property] = propertyName;\r\n\t\t}\r\n\t\treturn result;\r\n\t}\r\n\t\r\n\tprivate" +
                    " void SetExceptionFieldValue(Newtonsoft.Json.Linq.JObject jObject, string proper" +
                    "tyName, object value, string fieldName, Newtonsoft.Json.Serialization.IContractR" +
                    "esolver resolver, Newtonsoft.Json.JsonSerializer serializer)\r\n\t{\r\n\t\tvar field = " +
                    "System.Reflection.IntrospectionExtensions.GetTypeInfo(typeof(System.Exception))." +
                    "GetDeclaredField(fieldName);\r\n\t\tvar jsonPropertyName = resolver is Newtonsoft.Js" +
                    "on.Serialization.DefaultContractResolver ? ((Newtonsoft.Json.Serialization.Defau" +
                    "ltContractResolver)resolver).GetResolvedPropertyName(propertyName) : propertyNam" +
                    "e;\r\n\t\tvar property = System.Linq.Enumerable.FirstOrDefault(jObject.Properties()," +
                    " p => System.String.Equals(p.Name, jsonPropertyName, System.StringComparison.Ord" +
                    "inalIgnoreCase));\r\n\t\tif (property != null)\r\n\t\t{\r\n\t\t\tvar fieldValue = property.Va" +
                    "lue.ToObject(field.FieldType, serializer);\r\n\t\t\tfield.SetValue(value, fieldValue)" +
                    ";\r\n\t\t}\r\n\t}\r\n}");
            return this.GenerationEnvironment.ToString();
        }
    }
    
    #line default
    #line hidden
    #region Base class
    /// <summary>
    /// Base class for this transformation
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "15.0.0.0")]
    internal class JsonExceptionConverterTemplateBase
    {
        #region Fields
        private global::System.Text.StringBuilder generationEnvironmentField;
        private global::System.CodeDom.Compiler.CompilerErrorCollection errorsField;
        private global::System.Collections.Generic.List<int> indentLengthsField;
        private string currentIndentField = "";
        private bool endsWithNewline;
        private global::System.Collections.Generic.IDictionary<string, object> sessionField;
        #endregion
        #region Properties
        /// <summary>
        /// The string builder that generation-time code is using to assemble generated output
        /// </summary>
        protected System.Text.StringBuilder GenerationEnvironment
        {
            get
            {
                if ((this.generationEnvironmentField == null))
                {
                    this.generationEnvironmentField = new global::System.Text.StringBuilder();
                }
                return this.generationEnvironmentField;
            }
            set
            {
                this.generationEnvironmentField = value;
            }
        }
        /// <summary>
        /// The error collection for the generation process
        /// </summary>
        public System.CodeDom.Compiler.CompilerErrorCollection Errors
        {
            get
            {
                if ((this.errorsField == null))
                {
                    this.errorsField = new global::System.CodeDom.Compiler.CompilerErrorCollection();
                }
                return this.errorsField;
            }
        }
        /// <summary>
        /// A list of the lengths of each indent that was added with PushIndent
        /// </summary>
        private System.Collections.Generic.List<int> indentLengths
        {
            get
            {
                if ((this.indentLengthsField == null))
                {
                    this.indentLengthsField = new global::System.Collections.Generic.List<int>();
                }
                return this.indentLengthsField;
            }
        }
        /// <summary>
        /// Gets the current indent we use when adding lines to the output
        /// </summary>
        public string CurrentIndent
        {
            get
            {
                return this.currentIndentField;
            }
        }
        /// <summary>
        /// Current transformation session
        /// </summary>
        public virtual global::System.Collections.Generic.IDictionary<string, object> Session
        {
            get
            {
                return this.sessionField;
            }
            set
            {
                this.sessionField = value;
            }
        }
        #endregion
        #region Transform-time helpers
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void Write(string textToAppend)
        {
            if (string.IsNullOrEmpty(textToAppend))
            {
                return;
            }
            // If we're starting off, or if the previous text ended with a newline,
            // we have to append the current indent first.
            if (((this.GenerationEnvironment.Length == 0) 
                        || this.endsWithNewline))
            {
                this.GenerationEnvironment.Append(this.currentIndentField);
                this.endsWithNewline = false;
            }
            // Check if the current text ends with a newline
            if (textToAppend.EndsWith(global::System.Environment.NewLine, global::System.StringComparison.CurrentCulture))
            {
                this.endsWithNewline = true;
            }
            // This is an optimization. If the current indent is "", then we don't have to do any
            // of the more complex stuff further down.
            if ((this.currentIndentField.Length == 0))
            {
                this.GenerationEnvironment.Append(textToAppend);
                return;
            }
            // Everywhere there is a newline in the text, add an indent after it
            textToAppend = textToAppend.Replace(global::System.Environment.NewLine, (global::System.Environment.NewLine + this.currentIndentField));
            // If the text ends with a newline, then we should strip off the indent added at the very end
            // because the appropriate indent will be added when the next time Write() is called
            if (this.endsWithNewline)
            {
                this.GenerationEnvironment.Append(textToAppend, 0, (textToAppend.Length - this.currentIndentField.Length));
            }
            else
            {
                this.GenerationEnvironment.Append(textToAppend);
            }
        }
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void WriteLine(string textToAppend)
        {
            this.Write(textToAppend);
            this.GenerationEnvironment.AppendLine();
            this.endsWithNewline = true;
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void Write(string format, params object[] args)
        {
            this.Write(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void WriteLine(string format, params object[] args)
        {
            this.WriteLine(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Raise an error
        /// </summary>
        public void Error(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Raise a warning
        /// </summary>
        public void Warning(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            error.IsWarning = true;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Increase the indent
        /// </summary>
        public void PushIndent(string indent)
        {
            if ((indent == null))
            {
                throw new global::System.ArgumentNullException("indent");
            }
            this.currentIndentField = (this.currentIndentField + indent);
            this.indentLengths.Add(indent.Length);
        }
        /// <summary>
        /// Remove the last indent that was added with PushIndent
        /// </summary>
        public string PopIndent()
        {
            string returnValue = "";
            if ((this.indentLengths.Count > 0))
            {
                int indentLength = this.indentLengths[(this.indentLengths.Count - 1)];
                this.indentLengths.RemoveAt((this.indentLengths.Count - 1));
                if ((indentLength > 0))
                {
                    returnValue = this.currentIndentField.Substring((this.currentIndentField.Length - indentLength));
                    this.currentIndentField = this.currentIndentField.Remove((this.currentIndentField.Length - indentLength));
                }
            }
            return returnValue;
        }
        /// <summary>
        /// Remove any indentation
        /// </summary>
        public void ClearIndent()
        {
            this.indentLengths.Clear();
            this.currentIndentField = "";
        }
        #endregion
        #region ToString Helpers
        /// <summary>
        /// Utility class to produce culture-oriented representation of an object as a string.
        /// </summary>
        public class ToStringInstanceHelper
        {
            private System.IFormatProvider formatProviderField  = global::System.Globalization.CultureInfo.InvariantCulture;
            /// <summary>
            /// Gets or sets format provider to be used by ToStringWithCulture method.
            /// </summary>
            public System.IFormatProvider FormatProvider
            {
                get
                {
                    return this.formatProviderField ;
                }
                set
                {
                    if ((value != null))
                    {
                        this.formatProviderField  = value;
                    }
                }
            }
            /// <summary>
            /// This is called from the compile/run appdomain to convert objects within an expression block to a string
            /// </summary>
            public string ToStringWithCulture(object objectToConvert)
            {
                if ((objectToConvert == null))
                {
                    throw new global::System.ArgumentNullException("objectToConvert");
                }
                System.Type t = objectToConvert.GetType();
                System.Reflection.MethodInfo method = t.GetMethod("ToString", new System.Type[] {
                            typeof(System.IFormatProvider)});
                if ((method == null))
                {
                    return objectToConvert.ToString();
                }
                else
                {
                    return ((string)(method.Invoke(objectToConvert, new object[] {
                                this.formatProviderField })));
                }
            }
        }
        private ToStringInstanceHelper toStringHelperField = new ToStringInstanceHelper();
        /// <summary>
        /// Helper to produce culture-oriented representation of an object as a string
        /// </summary>
        public ToStringInstanceHelper ToStringHelper
        {
            get
            {
                return this.toStringHelperField;
            }
        }
        #endregion
    }
    #endregion
}