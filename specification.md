#NetRPC Specification

## 1. Overview
The key words "MUST", "MUST NOT", "REQUIRED", "SHALL", "SHALL NOT", "SHOULD", "SHOULD NOT", "RECOMMENDED", "MAY", and "OPTIONAL" in this document are to be interpreted as described in RFC 2119.

## 2. The ABC of NetRPC
Address denotes an enpoint on which to send messages to.
A binding defines what transport and what serialization technique that is used. Also defines additional communication requirements, for example message security
### 2.1 Message flow
#### 2.1.1 Server
The server must handle a two way message according to this flow:
	1.Transport in
		Receive message
			Accept message or return transport error code, such as a 500-response for an Http-request
		Parse message to a string representation
	2. Deserialization 
		Parse and validate string representation (typically deserialize to object)
			If error goto Serialization with error with code 100

	3. Locate Endpoint
		If not found return code 200
	4. Validate method and parameters
			If error return code 200
	5. Create or retreieve service instance 
			If error return code 300
	6. Invoke method
			If error return code 300
	7. Serialization
			Serialize response to string
				Set result property to method output (or null if void)
				Error response sets error property
				If error create new error response with error property
					If error go to Transport out

	8. Transport out
		Send reply and end incoming request as required by transport.


### 2.2 Transport
The protocol is to be transport agnostic.

#### 2.2.1 HTTP
Post to uri
Response of 500 if invalid post
#### 2.2.2 Inproc
#### 2.2.3 Socket
#### 2.2 .4 Other 
Ex ZMQ, or oneway over AMQP or MSMQ

### 2.2 Serialization
Any serialization format is ok, default is json

## 3. Request Object

## 4. Response Object
### 4.1 Result
#### 4.2.1 Void methods
### 4.2 Error
## 5. Error codes
Series 	Category
100 	Serialization errors
200		Service lookup/validation
300		Service activation/invocation
600		ClientSideErrors
## 6. Contract requirements
The only real constraint is no overload of actions. Ie. 
    inteface ISomething{
	    void Do(int id);
	    void Do(int id, string someData);
    }
is not valid. 
    interface ISomething{
        void Do(int id);
        void DoSomething(int id, string someData);
    }
Would be an acceptable alternative.