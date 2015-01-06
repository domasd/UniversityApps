#include <stdio.h>
#include <stdlib.h>
#include "graph.h"
#define MAX 17 


Graph* CreateGraph()
{
	Graph *ptr;
	ptr = (Graph *) (malloc(sizeof(Graph)));
	ptr->miestusk = 0;
	int i;
	for(i=0;i<MAX;i++){
		(ptr->cities[i])=CreateList();
	}
	return ptr;
}

List* CreateList()
{
	List *ptr;
	ptr = (List *) malloc(sizeof(List));

    ptr->head = NULL;
	ptr->tail = NULL;
	ptr->count = 0;	
	return (ptr);
}	

void AddFlight(int pradzia, int pabaiga,int isskridimo,int trukme, Graph **skrydziai ){
	AddelemList(  &(((*skrydziai)->cities)[pradzia])  , pabaiga, isskridimo, trukme); 
	
}

int CheckfullQu()
{
	struct qu *l; 
	l = (struct qu *) malloc(sizeof(struct qu));
	if (l == NULL)
	{
		return 1; // Error code 1
	}
	free(l);
	return 0; // OK, code 0
}	

int ExistValueList(List *ptr,int find) //Boolean 1 or 0
{
	int yra,duom,i;
	for (i=1; i <= (ptr->count); i++ )
	{
		if (GetValueList(ptr, i) == find) return 1;
	}
	return(0);
}

int AddelemList(List **ptr, int value, int laikas, int trukme )
{

	struct qu *temp; //Temporary
	temp = (struct qu  *) malloc(sizeof(struct qu));
	temp->value = value;
	temp->laikas = laikas;
	temp->trukme = trukme;
	temp->next = NULL;


	if ( ((*ptr)->head) == NULL)
	{
		((*ptr)->tail) = temp;
		(*ptr)->count = 1;           //tik siuo atveju
		(*ptr)->head = temp;         //tik siuo atveju
		((*ptr)->head)->next = NULL; //tik siuo atveju
	}
	else
	{
		((*ptr)->tail)->next = temp;  
		((*ptr)->tail) = temp;       
		((*ptr)->count)++;
	}
	return 0; // If ok
}

int RemoveelemQu(List **ptr,int *duom, int number)	//Remove the number'st element
{
	// if there is no requested elem  
	if 	(((*ptr)->count) < number) number = ((*ptr)->count); 
	// deleting the last elem
	if((*ptr)->count == 0) return 1; // if list has 0 elem
	else if( (*ptr)->count == 1 )
	{
		*duom = ((*ptr)->head)->value; // atiduodam skaiciu pries istrindami

		free((*ptr)->head);
		((*ptr)->tail) = NULL;
		((*ptr)->count)--;
	}
	else if( (*ptr)->count > 1 ) // if count > 1
	{
		int i;
		struct qu *l = (*ptr)->head;
		for(i = 0; i<(number-1); i++)
		{
			l=((l)->next);
			if (number == ((*ptr)->count)) // if requested is last elem, second from last deleting
			{
				if (i == (number-2)) ((*ptr)->tail) = l;
			}
		}

		*duom = l->value;
		if (number == 1)
		{
			((*ptr)->head) = ((*ptr)->head)->next;
		}
		free(l);
		((*ptr)->count)--;
		}
		return 0; // success
}

int GetLaikasList(List *ptr, int number)
{	// if there is no requested elem , getting the last
	int i;
	if 	((ptr->count) < number) number = (ptr->count); 
	struct qu *l = ptr->head;
	for(i = 0; i<(number-1); i++)
	{
		l=(l->next);
	}
	return l->laikas;
}

int GetAtvykimasList(List *ptr)
{	// if there is no requested elem , getting the last
	return ptr->atvykimas;
}

void SetAtvykimasList(List **ptr, int atvykimas)
{	// if there is no requested elem , getting the last
  (*ptr)->atvykimas = atvykimas;
}


int GetTrukmeList(List *ptr, int number)
{	// if there is no requested elem , getting the last
	int i;
	if 	((ptr->count) < number) number = (ptr->count); 
	struct qu *l = ptr->head;
	for(i = 0; i<(number-1); i++)
	{
		l=(l->next);
	}
	return l->trukme;
}


int GetValueList(List *ptr, int number)
{	// if there is no requested elem , getting the last
	int i;
	if 	((ptr->count) < number) number = (ptr->count); 
	struct qu *l = ptr->head;
	for(i = 0; i<(number-1); i++)
	{
		l=(l->next);
	}
	return l->value;
}



int GetelemList(List *ptr, int *duom, int number)
{	// if there is no requested elem , getting the last
	int i;
	if 	((ptr->count) < number) number = (ptr->count); 
	struct qu *l = ptr->head;
	for(i = 0; i<(number-1); i++)
	{
		l=(l->next);
	}
	*duom = l->value;
	return 0;
}




int ChangeelemList(List **ptr, int duom, int number)
{	
	int i;
	// if there is no requested elem , getting the last
	if 	(((*ptr)->count) < number) number = ((*ptr)->count); 
	struct qu *l = (*ptr)->head;
	for(i = 0; i<(number-1); i++)
	{
		l=(l->next);
	}
	l->value = duom;
	return 0;
}
int SwapelemList(List **ptr, int elem1, int elem2)
{	
	int i;
	if ((*ptr)->count < 2) return 1; // error
	// if there is no requested numbers...
	if 	(  ((*ptr)->count) < elem1 && ((*ptr)->count) < elem2 )
	{
		 if (elem2 > elem1) 
		 {
		 	elem2 = (*ptr)->count; // getting the last
		 	elem1 = ((*ptr)->count)-1; // and second from the last
		 }
		 else
		 {
		 	elem1 = (*ptr)->count; // getting the last
		 	elem2 = ((*ptr)->count)-1; // and second from the last
		 }
	}


	struct qu *l1 = (*ptr)->head; int duom;
	struct qu *l2 = (*ptr)->head;

	for(i = 0; i<(elem1-1); i++)
	{
		l1=(l1->next);
	}
	for(i = 0; i<(elem2-1); i++)
	{
		l2=(l2->next);
	}
	duom = l1->value; l1->value = l2->value; l2->value = duom; // SWAP the values
	return 0;
}

////// QUICK SORT //////

int Partition(List **ptr, int start, int end) // STATIC
{
  int pivot; int partitionIndex = start;
  GetelemList(*ptr, &pivot , end); // make the last element to the pivot
  int i,duom; 

  for( i = start; i < end; i++ )
  {
    GetelemList(*ptr, &duom , i);
    if ( duom >= pivot) // in what order?
    { 
      SwapelemList(ptr, i, partitionIndex);
      partitionIndex++;
    }
  }
  SwapelemList(ptr,partitionIndex,end);
  return partitionIndex;
}

void QuicksortList(List **ptr, int start, int end)
{ 
  int i,weight;
  if (start < end)
  {
    int pIndex = Partition(ptr,start,end);
    QuicksortList(ptr,start,pIndex-1);
    QuicksortList(ptr,pIndex+1,end);
  }

}

////// -------- //////


int CountList(List *ptr)
{
	int quantity=0;
	quantity = ptr->count;
	return(quantity);
}

int SumList(List *ptr)
{
	int i, sum=0; struct qu *l = ptr->head;
	for (i=1; i <= (ptr->count); i++ )
	{
		sum += l->value;
		l=l->next;
	}
	return(sum);
}
/*
int CopyList(List **ptr, List *ptr2)
{
	int i,duom;
	DeleteelemsList(ptr);
	for (i=1; i <= (ptr2)->count; i++ )
	{

		GetLaikasList(skrydziai->cities[i], g)
		GetValueList()
		GetTrukmeList()
		GetAtvykimasList()


		AddelemList(ptr, pabaiga, isskridimo, trukme); 
		//AddelemList(ptr,duom);
	}
	return 0;
}*/
int ExistelemList(List *ptr,int find) //Boolean 1 or 0
{
	int yra,duom,i;
	for (i=1; i <= (ptr->count); i++ )
	{
		GetelemList(ptr,&duom,i);
		if (duom == find) return 1;
	}
	return(0);
}

int FreeList(List **ptr)
{
	struct qu *next;
	while( (*ptr)->count != 0) // elementu naikinimas
	{
		next = ((*ptr)->head)->next;
		free(((*ptr)->head));
		((*ptr)->head) = next;
		((*ptr)->count)--;
	}
	free(*ptr);

}

int DeleteelemsList(List **ptr)
{
	struct qu *next;
	while( (*ptr)->count != 0) // elementu naikinimas
	{
		next = ((*ptr)->head)->next;
		free(((*ptr)->head));
		((*ptr)->head) = next;
		((*ptr)->count)--;
	}
}









