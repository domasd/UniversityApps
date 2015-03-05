Code example I used: https://msdn.microsoft.com/en-us/library/windows/desktop/ms738545%28v=vs.85%29.aspx	
Also: http://uosis.mif.vu.lt/~vilkas/
Author: dziaugys.com

This program is created for "Computer networking" lecture. The focus was to learn socket programming
and to create a program which effectively uses windows socket API.

Program name is "Auction". It can create several auctions, and a multiple customers can connect to server to
bid on them. 

Server app has a Params.txt for PORT number and Auctions with starting price. Additionaly, user can create auctions
directly from console line and it will be automatically saved. Creation: [AuctionName] [StartingPrice]

Client app starts with a console parameter called IP and PORT. It connects to the indicated server with given port.
Commands: list - lists all the active auctions with Id's; [AuctionId] [Bid]

Known flaws: it could be a pitfall if to parallel threads will acess Server's save auction to file function.