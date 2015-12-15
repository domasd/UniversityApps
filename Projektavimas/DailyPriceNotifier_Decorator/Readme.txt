This is a simple program i wrote for getting notified of price changes (if it gets below marked).
It is written for Design Patterns lecture, I used decorator pattern, but I actually use it very often for my own needs.

It notifies in three platforms:
Email
Nirvana Task management software
Windows icon tray

All configuration is done in App.config

The XML of URLS must match the following scheme
<Urls>
	<Url Parser="ParserName">URLOfProduct</Url>
</Urls>

Please use on your own risk.

Todo list:
-Make parsers dynamic load (it should be loaded dynamically, e.g. through reflection)
-Make possibility to add different price limit (it means different products)