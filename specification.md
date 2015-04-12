#NetRPC Specification

## 1. Overview
The key words "MUST", "MUST NOT", "REQUIRED", "SHALL", "SHALL NOT", "SHOULD", "SHOULD NOT", "RECOMMENDED", "MAY", and "OPTIONAL" in this document are to be interpreted as described in RFC 2119.

## 2. The ABC of NetRPC
Address denotes an enpoint on which to send messages to
### 2.1 Transport
### 2.1.1 HTTP
Post to uri
Response of 500 if invalid post
### 2.1.2 Inproc
### 2.1.3 Socket
### 2.1.4 Other 
Ex ZMQ, or oneway over AMQP or MSMQ

### 2.2 Serialization
Any serialization format is ok, default is json

## 3. Request Object

## 4. Response Object
### 4.1 Result
#### 4.2.1 Void methods
### 4.2 Error