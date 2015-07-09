Imports System.ComponentModel

Public Class JsonOrDefaultModelBinder
    Inherits DefaultModelBinder

#Region " Overriden Methods "

    ''' <summary>
    ''' Overridden GetPropertyValue Function.
    ''' </summary>
    Protected Overrides Function GetPropertyValue(ByVal controllerContext As ControllerContext, ByVal bindingContext As ModelBindingContext, ByVal propertyDescriptor As PropertyDescriptor, ByVal propertyBinder As IModelBinder) As Object

        'Try to get the value as a JSON model.
        Dim value = BindJsonModel(controllerContext, bindingContext)

        If (value Is Nothing) Then

            'No value was returned from the BindJsonModel function. So try to get it the default way.
            value = propertyBinder.BindModel(controllerContext, bindingContext)

        End If

        If bindingContext.ModelMetadata.ConvertEmptyStringToNull AndAlso Equals(value, [String].Empty) Then
            Return Nothing
        End If

        Return value

    End Function

#End Region

#Region " Private Methods "

    ''' <summary>
    ''' Attempt to bind the JSON data to the model.
    ''' </summary>
    Private Shared Function BindJsonModel(ByVal controllerContext As ControllerContext, ByVal bindingContext As ModelBindingContext)

        If (bindingContext.ModelType.IsPrimitive OrElse bindingContext.ModelType.IsValueType OrElse bindingContext.ModelType Is GetType(String)) Then

            'Property is not a complex type so do not attempt to deserialize.
            Return Nothing

        End If

        Try

            Dim serializedValue = controllerContext.HttpContext.Request(bindingContext.ModelName)

            'Attempt to deserialize the value.
            Return Newtonsoft.Json.JsonConvert.DeserializeObject(serializedValue, bindingContext.ModelType)

        Catch ex As Exception

            'Exception. So just return nothing and the calling function will perform further processing.
            Return Nothing

        End Try

    End Function

#End Region

End Class