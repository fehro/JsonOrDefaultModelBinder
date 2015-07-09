# JsonOrDefaultModelBinder
A replacement MVC model binder that allows properties to be in serialized JSON form allowing Angular / Knockout whatever you want to manage segments of your MVC view's model.
==============

I apologise for this being all in VB.net. Also the example below is just a simple guideline. I know it is not how you should ng :)

Usage
--------------
In the Application_Start of your Global.asax file simply add the line below to change your default model binder.

    ModelBinders.Binders.DefaultBinder = New JsonOrDefaultModelBinder()

Example
--------------
Assume you have two models as below.

    Public Class PostcodeRange
	
	    Public Property FromPostcode As Integer
		
		Public Property ToPostcode As Integer
		
	End Class

    Public Class PostcodesZone
	
	    Public Property Code As String
		
		Public Property PostcodeRanges As List(Of PostcodeRange)
		
	End Class
	
If you wanted to say handle the adding / removal of the PostcodeRanges list via Angular but the Code property in your Razor view then this can be done as below.

**Razor Template**

    @Using (Html.BeginForm())

		@Html.TextBoxFor(Function(x) x.Code)
	
		<div ng-app ng-init="postcodeRanges = @Newtonsoft.Json.JsonConvert.SerializeObject(Model.PostcodeRanges)">
		
			<!-- Put your Angular logic here -->

    		<input type="hidden" name="PostcodeRanges" value="{{postcodeRanges}}" />

		</div>
		
		<input type="submit" value="Save" />
	
	End Using
	
**View Model**

	<HttpPost>
	Function Edit(model As PostcodesZone) As ActionResult
	
		'Do your saving here.

		Return View("Edit", model)

	End Function
