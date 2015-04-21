/*
Code example I used: https://msdn.microsoft.com/en-us/library/windows/desktop/ms738545%28v=vs.85%29.aspx
Author: dziaugys.com
*/

#ifndef WIN32_LEAN_AND_MEAN
#define WIN32_LEAN_AND_MEAN
#endif

#define _WINSOCK_DEPRECATED_NO_WARNINGS
#define _CRT_SECURE_NO_WARNINGS

#include <windows.h>
#include <winsock2.h>
#include <ws2tcpip.h>
#include <iphlpapi.h>
#include <stdio.h>
#include <stdlib.h>
#include <float.h >
#include <math.h>

//Const
#define DEFAULT_PORT (Port)
#define DEFAULT_BUFLEN 1024
#define MAX_CLIENTS 100
#define MAX_AUCTIONS 100
#define ParamsTxt "Params.txt"

//Functions declaration
DWORD WINAPI SocketHandler(void*); // for multithreading
char* Auction(float Amount); // for auction purposes
void SendNewPrice(); //for sending msg to all clients
void AddToClientsSocketArray(void *csock); // For adding sockets to array
DWORD WINAPI NewAuctionListener(); //For listening user input for creating new auctions anytime
int CreateNewAuction(char *title[], float price); // for creating actual new auction
char* List(); // for sending user the info about all auctions
void AppendParams(char *title[], float price); //Save created auctions

//Global vars
char receiveBuffer[DEFAULT_BUFLEN];
int* connectedSockets[MAX_CLIENTS];
int iResult, iSendResult;
char Port[10];


typedef struct AuctionItems
{
	char  title[50];
	float price;
} AuctionItem;

AuctionItem *AuctionItems[MAX_AUCTIONS];
AuctionItem *ptr;

#pragma comment(lib, "Ws2_32.lib")

int main() {

	FILE *fParam;
	char buff[255]; //One row to read from file
	float readAmount;

	if ((fParam = fopen(ParamsTxt, "r")) == NULL){ //read for creating saved Auctions
		printf("Error retrieving the parameters for program\n");
		return 1;
	}
	if (fscanf(fParam, "PORT = %s\n", buff) != EOF){
		strcpy(Port, buff);
	}
	else return 1;

	fgets(buff, 255, fParam); // skip one line (comment in Params.txt)
	while (fscanf(fParam, "%s %f\n", &buff, &readAmount) != EOF){
		CreateNewAuction(buff, readAmount);

	}
	fclose(fParam);

	WSADATA wsaData; // Information about the wsocket implementation
	SOCKET ListenSocket = INVALID_SOCKET;

	struct addrinfo *result = NULL, *ptr = NULL, hints;
	struct sockaddr_in clientAddres;
	int	clientAddres_size = sizeof(SOCKADDR);

	// Initialize Winsock
	iResult = WSAStartup(MAKEWORD(2, 2), &wsaData); //v2.2 winsock
	if (iResult != 0) {
		printf("WSAStartup failed: %d\n", iResult);
		return 1;
	}

	ZeroMemory(&hints, sizeof(hints)); // Fills a block with zeros, &hints  is pointer
	ZeroMemory(&receiveBuffer, sizeof(receiveBuffer));

	hints.ai_family = AF_INET; //IPv4 protokolas 
	hints.ai_socktype = SOCK_STREAM;
	hints.ai_protocol = IPPROTO_TCP;
	hints.ai_flags = AI_PASSIVE; // will use returned socket address struct in a call to the bind function

	// Resolve the local address and port to be used by the server
	iResult = getaddrinfo(NULL, DEFAULT_PORT, &hints, &result); // 
	if (iResult != 0) {
		printf("getaddrinfo failed: %d\n", iResult);
		WSACleanup();
		return 1;
	}

	// Create a SOCKET for connecting to server
	ListenSocket = socket(result->ai_family, result->ai_socktype, result->ai_protocol);
	if (ListenSocket == INVALID_SOCKET) {
		printf("Error at socket(): %ld\n", WSAGetLastError());
		freeaddrinfo(result);
		WSACleanup();
		return 1;
	}

	//Binding the socket
	iResult = bind(ListenSocket, result->ai_addr, (int)result->ai_addrlen); // Result is from getaddrinfo function, which has local address, port etc.
	if (iResult == SOCKET_ERROR) {
		printf("bind failed with error: %d\n", WSAGetLastError());
		freeaddrinfo(result);
		closesocket(ListenSocket);
		WSACleanup();
		return 1;
	}

	freeaddrinfo(result); // No longer needed

	if (listen(ListenSocket, MAX_CLIENTS) == SOCKET_ERROR) { // SOMAXCONN -  constant that instructs the Winsock provider to allow a maximum reasonable number
		printf("Listen failed with error: %ld\n", WSAGetLastError());
		closesocket(ListenSocket);
		WSACleanup();
		return 1;
	}

	int* csock; //client sock representation


	CreateThread(0, 0, &NewAuctionListener, 0, 0, 0);
	for (;;){
		printf("Waiting for connection...\n");
		csock = (int*)malloc(sizeof(int));
		// Accept a client socket
		*csock = accept(ListenSocket, (SOCKADDR*)&clientAddres, &clientAddres_size);
		if (*csock == INVALID_SOCKET) {
			printf("accept failed: %d\n", WSAGetLastError());
			closesocket(ListenSocket);
			WSACleanup();
			return 1;
		}
		else{
			printf("-----------------\n");
			printf("Client connected - %s  \n", inet_ntoa(clientAddres.sin_addr));
			printf("-----------------\n");
			AddToClientsSocketArray((void*)csock);
			CreateThread(0, 0, &SocketHandler, (void*)csock, 0, 0);

		}
	}
	WSACleanup();
	return 0;
}

DWORD WINAPI SocketHandler(void* lp){
	int *csock = (int*)lp;

	char buffer[DEFAULT_BUFLEN];
	int bufflen = DEFAULT_BUFLEN;
	int bytecount;
	int bytecountRecv;

	for (;;){
		bytecountRecv = 0;
		ZeroMemory(buffer, sizeof(buffer));
		if ((bytecountRecv = recv(*csock, buffer, bufflen, 0)) == SOCKET_ERROR){ // Receiving the actual data
			fprintf(stderr, "Connection lost %d\n", WSAGetLastError());
			goto FINISH;
		}
		else {
			//TODO print list of current auctions
			char response[DEFAULT_BUFLEN];

			char *firstToken; //The first parameter user sent
			firstToken = strtok(buffer, " ");

			if (strcmp(firstToken, "list") == 0){ // Client wants to get the list of all auctions
				strcpy(response, List());
			}
			else{ //Client wants to bid
				int AuctionId;
				float Amount;

				AuctionId = strtol(firstToken, NULL, 10); //Get int from string
				Amount = strtod(strtok(NULL, " "), NULL);

				strcpy(response, Auction(Amount, AuctionId));
			}

			if ((bytecount = send(*csock, response, strlen(response), 0)) == SOCKET_ERROR){
				fprintf(stderr, "Error sending data to client %d\n", WSAGetLastError());
				goto FINISH;
			}
		}
	}

FINISH:
	closesocket(csock);
	return 0;
}

DWORD WINAPI NewAuctionListener(){

	char line[80];
	char title[50];
	char *PtrPrice;
	float price;
	int argc;

	for (;;){
		fgets(line, 80, stdin);
		if (line != NULL && line[0] != '\n'){
			// Proccess the data user entered
			strcpy(title, strtok(line, " "));
			PtrPrice = strtok(NULL, " "); //This is because using directly atof() throws error

			if (PtrPrice == NULL || (price = atof(PtrPrice)) == 0 || price > FLT_MAX || title == NULL){
				printf("Bad input. Usage: <AuctionTitle> <StartingPrice>\n");
			}
			else{
				CreateNewAuction(title, price);
				AppendParams(title, price); //Save created auction to a file
			}
		}
		else {
			printf("Bad input. Usage: <AuctionTitle> <StartingPrice>\n");
		}
	}


}

int CreateNewAuction(char *title[], float price){
	int i;
	for (i = 0; i < MAX_AUCTIONS; i++){
		if (AuctionItems[i] == NULL){
			AuctionItems[i] = (AuctionItem *)malloc(sizeof(AuctionItem));
			AuctionItems[i]->price = price;
			strcpy(AuctionItems[i]->title, title);

			printf("Created new auction:\n");
			printf("    [%d] Price %f\n", i + 1, AuctionItems[i]->price);
			printf("    [%d] Title %s\n", i + 1, AuctionItems[i]->title);

			return 0; //Ok
		}
		else {
			if (i + 1 == MAX_AUCTIONS){
				printf("Max number of auctions can't be exceeded\n");
				return 1; //Error
			}
		}
	}
}

char* Auction(float Amount, int AuctionId){

	char *response = calloc(DEFAULT_BUFLEN, sizeof(char));
	if (AuctionId <= 0){
		return "Requested auction does not exist!";
	}
	AuctionItem *requestedItem = AuctionItems[AuctionId - 1];
	if (requestedItem == NULL){
		return "Requested auction does not exist!";
	}

	if (floorf(Amount * 100) / 100  > requestedItem->price){
		strcpy(response, "\nYou are the highest bidder!");
		requestedItem->price = Amount;
		printf("BID REGISTERED: Amount: %2.2f Auction: %s\n", Amount, requestedItem->title);
		SendNewPrice(AuctionId - 1);
	}
	else{
		printf("Bid attempt: %2.2f\n", Amount);
		strcpy(response, "\nYou are NOT the highest bidder!");
	}

	char temp[DEFAULT_BUFLEN]; //Bid user sent
	sprintf(temp, "%.2f", Amount); //To save float as string
	char tempCurr[DEFAULT_BUFLEN];  //Actual amount now
	sprintf(tempCurr, "%.2f", requestedItem->price); //To save float as string

	strcat(response, "\nAuction you are bidding for: ");
	strcat(response, requestedItem->title);
	strcat(response, "\nBid you sent: ");
	strcat(response, temp);
	strcat(response, "\nCurrent price: ");
	strcat(response, tempCurr);
	return response;

}

char* List(){
	char *response = calloc(DEFAULT_BUFLEN, sizeof(char));
	strcpy(response, "\nAll auctions:");
	for (int i = 0; i < MAX_AUCTIONS; i++){
		if (AuctionItems[i] != NULL){
			char temp[80];
			sprintf(temp, "\n -- ID: %d Title : %s Current Price: %.2f ", i + 1, AuctionItems[i]->title, AuctionItems[i]->price);
			strcat(response, temp);
		}
		else {
			break;
		}

	}
	return response;
}


void SendNewPrice(int AuctionId){
	int i = 0;
	char msg[DEFAULT_BUFLEN]; // for message to all clients
	//char tempText[80];
	sprintf(msg, "\n !!! NEW AUCTION PRICE OF %s AUCTION: %.2f !!!", AuctionItems[AuctionId]->title, AuctionItems[AuctionId]->price);
	//strcpy(msg, tempText);

	for (i; i < MAX_CLIENTS; i++){
		if (connectedSockets[i] != NULL){
			if (send(*connectedSockets[i], msg, strlen(msg), 0) == SOCKET_ERROR){ //Sending the message to all clients (connected sockets)
				fprintf(stderr, "Error sending data to client (might be disconnected) %d\n", WSAGetLastError());
				connectedSockets[i] = NULL; //remove client from connected clients array
			}
		}
	}
}


void AddToClientsSocketArray(void* lp){
	int *csock = (int*)lp;
	int i = 0;
	for (i; i < MAX_CLIENTS; i++){
		if (connectedSockets[i] == NULL){
			connectedSockets[i] = csock;
			break;
		}
	}
}

void AppendParams(char *title[], float price){
	FILE *fParam;

	if ((fParam=fopen(ParamsTxt, "a+")) != NULL){
		fprintf(fParam, "\n%s %f", title, price);
		fclose(fParam);
	}
	else{
		printf("Could not write to the parameter file!");
	}
}





