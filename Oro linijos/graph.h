#ifndef graph_H
#define graph_H
#define myType int
#define MAX 17 

typedef struct S S;
typedef struct graph Graph;
typedef struct que List;
 struct S
 {
 	int reiksme;
 	int marked;
 	int buves;
 	int atvykimolaikas;

 };
 struct graph
 {
 	int miestusk;
 	List *cities[MAX]; // List'u t.y. miestu array
 	//List* cities;
 };



//####################	
 struct qu 
	{
		myType value;
		int laikas;
		int trukme;
		struct qu *next;
	};
//####################	
		struct que 
		{
			struct qu *head;
			struct qu *tail;
			int atvykimas;
			int count;
		};
	
int AddelemList(List **ptr, int value, int laikas, int trukme );	
int ExistValueList(List *ptr,int find);
int GetValueList(List *ptr, int number);
int GetTrukmeList(List *ptr, int number);
int GetLaikasList(List *ptr, int number);
int CountList(List *ptr);
int GetAtvykimasList(List *ptr);
void SetAtvykimasList(List **ptr, int atvykimas);
Graph* CreateGraph();

int FreeQu(List **ptr);


int RemoveelemQu(List **ptr,int *duom, int number); 
int GetelemList(List *ptr, int *duom, int number); 
int ChangeelemList(List **ptr, int duom, int number);
int CheckfullList();
int CountList(List *ptr);
int SumList(List *ptr);
//int CopyList(List **ptr, List* ptr2); // is ptr2 i ptr
int ExistelemList(List *ptr,int find);
List* CreateList();
void QuicksortList(List **ptr, int start, int end);

	

	
	
	
#endif
