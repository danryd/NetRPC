# NetRPC
Because programming to interfaces is great

##Motivation
I think there's a place for RCP-style of integration in this world, and I do like programming to interfaces. Therefore I don't think REST fulfills all necessary roles. 
So why roll your own? The options arn't that great for all contexts. SOAP-type of services are kind of clunky. WCF is powerful but tweaking messagesize isn't really creating business value, also the runtime feels kind of buggy in part due to wierd defaults. A natural alternative would be JSON-Rpc, however that spec doesn't allow header data which I consider a must.