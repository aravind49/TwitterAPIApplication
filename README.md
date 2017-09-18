# TwitterAPIApplication
TwitterAPITestApplication

Application is developed in Visual studio 2015. To create the application, I used the WebApi (MVC + Webapi) template,
but removeds some of the default files provided as part of templte. Using one of the online forum, converted the TwitterAPI json response
to classes. Included all the generated classes in TwitterAPIResponse.cs model.

To avoid confusion, Below mentioning the files I have added or modified.
	1) Test Project
	2) Model Project --Added all the models used in this project
	3) TwitterAPIApplication
		a. Content\TwitterAPI.css
		b. Controllers\HomeController.cs
		c. ProxyLayer\TwitterAPIProxy.cs
		d. RestAPIClient\RestClient.cs
		e. Scripts\Twitter.js
		f. Views\Home\Index.cshtml
		g. Views\Home\Twitter.cshtml
Web.config
