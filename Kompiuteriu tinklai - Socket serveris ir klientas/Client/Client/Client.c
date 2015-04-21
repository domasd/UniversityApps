/*
Code example I used: https://msdn.microsoft.com/en-us/library/windows/desktop/ms738545%28v=vs.85%29.aspx	
Also: http://uosis.mif.vu.lt/~vilkas/examples/echo_client.c
Author: dziaugys.com
*/

#define _CRT_SECURE_NO_WARNINGS

#ifndef WIN32_LEAN_AND_MEAN
#define WIN32_LEAN_AND_MEAN
#endif

#include <windows.h>
#include <winsock2.h>
#include <ws2tcpip.h>
#include <iphlpapi.h>
#include <stdio.h>
#include <float.h>
#include <stdlib.h>    

#define DEFAULT_BUFLEN 1024

DWORD WINAPI Listener(void*);

char receiveBuffer[DEFAULT_BUFLEN];
int iResult, iSendResult; // for error 
float bid;

#pragma comment (lib, "Ws2_32.lib")
#pragma comment (lib, "Mswsock.lib")
#pragma comment (lib, "AdvApi32.lib")

int main(int argc, char* argv[]) {

	char port[10];
	char host_name[10];

	if (argc != 3){
		fprintf(stderr, "USAGE: %s <ip> <port>\n", argv[0]);
		exit(1);
	}

	strcpy(port,argv[2]);
	strcpy(host_name, argv[1]);

	if ((atoi(port) < 1) || (atoi(port) > 65535)){
		printf("Invalid port specified.\n");
		exit(1);
	}




	WSADATA wsaData;
	SOCKET ConnectSocket = INVALID_SOCKET;

	struct addrinfo *result = NULL,
		*ptr = NULL,
		hints;

	// Initialize Winsock
	iResult = WSAStartup(MAKEWORD(2, 2), &wsaData);
	if (iResult != 0) {
		printf("WSAStartup failed with error: %d\n", iResult);
		goto FINALLY;
	}

	ZeroMemory(&hints, sizeof(hints)); // Fills with 0
	hints.ai_family = AF_INET; //AF_UNSPEC;
	hints.ai_socktype = SOCK_STREAM;
	hints.ai_protocol = IPPROTO_TCP;

	// Resolve the server address and port
	iResult = getaddrinfo(host_name, port, &hints, &result);
	if (iResult != 0) {
		printf("getaddrinfo failed: %d\n", iResult);
		WSACleanup();
		goto FINALLY;
	}

	// Attempt to connect to an address until one succeeds
	for (ptr = result; ptr != NULL; ptr = ptr->ai_next) {

		// Create a SOCKET for connecting to server
		ConnectSocket = socket(ptr->ai_family, ptr->ai_socktype,
			ptr->ai_protocol); //
		if (ConnectSocket == INVALID_SOCKET) {
			printf("socket failed with error: %ld\n", WSAGetLastError());
			WSACleanup();
			goto FINALLY;
		}

		// Connect to server.
		iResult = connect(ConnectSocket, ptr->ai_addr, (int)ptr->ai_addrlen); //trying to connect, passing connect socket
		if (iResult == SOCKET_ERROR) {
			closesocket(ConnectSocket);
			ConnectSocket = INVALID_SOCKET;
			continue;
		}
		break;
	}

	freeaddrinfo(result); // not needed anymore


	if (ConnectSocket == INVALID_SOCKET) {
		printf("Unable to connect to server!\n");
		WSACleanup();
		goto FINALLY;
	}

	//create listener thread
	CreateThread(0, 0, &Listener, (void*)&ConnectSocket, 0, 0); //Listens to the server, and prints the text

	//Participating in auction
	printf("Enter \"list\" to get all auctions info\n");
	printf("Enter [AuctionId] [Amount] to bid\n");
	char line[255];
	char toSend[DEFAULT_BUFLEN];
	
	char *firstParam;
	char *scndParam;
	for (;;){
		fgets(line, 80, stdin);
		if (line != NULL && line[0] != '\n'){
			firstParam = strtok(line, " \n");
			if (strcmp(firstParam,"list") == 0){
				strcpy(toSend, firstParam);
			}
			else {
				scndParam = strtok(NULL, " \n");
				if (firstParam != NULL && scndParam != NULL
					&& strtol(firstParam, NULL, 10) != 0 && atof(scndParam) != 0
					&& atof(scndParam) <= FLT_MAX){
					strcpy(toSend, line);
					strcat(toSend, " ");
					strcat(toSend, scndParam);
				}
				else{
					printf("Bad input. Try again.\n");
					continue;
				}				
			}
		}
		else{
			continue;
		}
		// Send an initial buffer
		iResult = send(ConnectSocket, toSend, (int)strlen(toSend), 0);
		if (iResult == SOCKET_ERROR) {
			printf("send failed with error: %d\n", WSAGetLastError());
			closesocket(ConnectSocket);
			WSACleanup();
			goto FINALLY;
		}
	}

	// shutdown the connection since no more data will be sent
	iResult = shutdown(ConnectSocket, SD_SEND);
	if (iResult == SOCKET_ERROR) {
		printf("shutdown failed with error: %d\n", WSAGetLastError());
		closesocket(ConnectSocket);
		WSACleanup();
		goto FINALLY;
	}
	// cleanup
	WSACleanup();


FINALLY:
	getch();
	return 0;
}

DWORD WINAPI Listener(void* lp){
	int *csock = (int*)lp;
	for (;;){
		ZeroMemory(receiveBuffer, DEFAULT_BUFLEN); //if different byte count is sent each time, it will may corrupt data
		iResult = recv(*csock, receiveBuffer, DEFAULT_BUFLEN, 0);
		if (iResult > 0){
			printf("Server returned: %s\n", receiveBuffer);
			//printf("Bytes received: %d\n", iResult);
		}
		else if (iResult == 0){
			printf("Connection closed\n");
			goto FINALLY;
		}
		else{
			printf("recv failed with error (Servermight be down): %d\n", WSAGetLastError());
			goto FINALLY;
		}
	}
FINALLY:
	closesocket(csock);
	return 0;
}