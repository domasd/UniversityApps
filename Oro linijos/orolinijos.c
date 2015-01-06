#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include "graph.h"
int countOfCity=0; 
int Scount=0; 
char miestai[MAX][20]; // Array pakeist miestam i skaiciukus, iki 17miestu po 20 simboliu


int ThereIsInArray(char patikrint[20], char miestai[MAX][20], int *g){
	int i;
	for( i=0; i<countOfCity; i++){
		if (strcmp(miestai[i], patikrint) == 0){
			*g = i;
			return 1; // yra
		}
	}
	return 0; //nera
}
int* CopyCityIdArray(int kopijuot[], int saltinis[]){
    int i;
    for( i=0; i<MAX; i++){
        kopijuot[i]=saltinis[i];
    }
    return kopijuot;
}

int* CreateCityIdArray(int array[MAX]){
    int i;
    for( i=0; i<MAX; i++){
        array[i] = -1;
    }
    return array;
}

void AddToCityIdArray(int array[MAX], int ID){
    int i;
    for( i=0; i<MAX; i++){
        if(array[i] == -1){
            array[i] = ID;
            return;
        }
    }    
}



int kieklaukt(int atvykimas, int isvykimas){
    if(atvykimas == isvykimas){
        return 0;
    }
    else if(atvykimas > isvykimas){
        return (1440-(atvykimas-isvykimas));
    }
    else if(atvykimas < isvykimas){
        return( isvykimas - atvykimas);
    }
}

int realitrukme(int atvykimas, int isvykimas, int trukme){
    if(atvykimas == -1) atvykimas = isvykimas;
    int laukti = kieklaukt(atvykimas,isvykimas);
    //printf("Atvykimas - %d Isvykimas - %d Laukti - %d\n",atvykimas,isvykimas,laukti);
    return trukme + laukti;
}

int realitrukmeBrute(int atvykimas, int isvykimas, int trukme, int startas){
    if(atvykimas == -1 || startas == 1 ) atvykimas = isvykimas;
    int laukti = kieklaukt(atvykimas,isvykimas);
    //printf("Atvykimas - %d Isvykimas - %d Laukti - %d\n",atvykimas,isvykimas,laukti);
    return trukme + laukti;
}

int paroslaikas(int minutes){
    return (minutes%1440);
}

//######################################################################################################
//######################################################################################################
//######################################################################################################
//######################################################################################################
void ShortestPath(Graph *skrydziai,int pradinis, S* s){
    int i,g,z; // pagalbiniai
    int min = 0x7FFF;
    int minID;
    int reali;
    int ID, IDliste;
    int selected;

    selected = pradinis; 

    while (Scount < countOfCity){

        for (i=1; i<=CountList(skrydziai->cities[selected]); i++ ){ // praeinam pro visas virsunes sakas

            reali = realitrukme((s+selected)->atvykimolaikas, 
            GetLaikasList(skrydziai->cities[selected], i) , 
            GetTrukmeList(skrydziai->cities[selected], i)) ;

            // keiciam virsuniu reiksmes 
            ID = GetValueList(skrydziai->cities[selected], i);

            if( (((s+selected)->reiksme) + reali) < (((s+ID)->reiksme)) && ((s+ID)->marked) == 0){
                ((s+ID)->reiksme) = ((s+selected)->reiksme) + reali;
                ((s+ID)->buves) = selected;
                if((s+((s+ID)->buves))->atvykimolaikas == -1)
                   
                    ((s+ID)->atvykimolaikas) = paroslaikas((s+ID)->reiksme+GetLaikasList(skrydziai->cities[selected], i));
                if((s+((s+ID)->buves))->atvykimolaikas != -1)
                    ((s+ID)->atvykimolaikas) = paroslaikas(GetLaikasList(skrydziai->cities[selected], i) + 
                                                            GetTrukmeList(skrydziai->cities[selected],i));
            }

            
        }
        //Jei grafo virsune neturi saku i kitas
        if(CountList(skrydziai->cities[selected]) == 0 ){


            minID= -1;
            for(g = 0;g<countOfCity;g++){
                if( ((s+g)->reiksme)<min && (s+g)-> marked == 0    ){
                    min = ((s+g)->reiksme);
                    minID = g; 
                }
            }
            if (minID == -1){
             break;
            }
            //Pridedam i pazymeta sarasa
            ((s+minID)->marked) = 1;
            Scount++;

           
            // Keiciam selected ir min reiksmes
            min = 0x7FFF;
            selected = minID;

            continue;



        }

        if (i-1 == CountList(skrydziai->cities[selected]) ){
             //Ieskom dabartinio maziausio nepazymeto esancio s
            for(g =0;g<countOfCity;g++){
                if( min>((s+g)->reiksme) && (s+g)->marked == 0   ){
                    min = ((s+g)->reiksme);
                    minID = g; 
                }
            }
            

            
            for(z=1;z<=CountList(skrydziai->cities[selected]); z++ ){
                if(GetValueList(skrydziai->cities[selected], z) == minID){
                    IDliste = minID;
                }
            }

            
            ((s+minID)->marked) = 1;
            Scount++;

            
            min = 0x7FFF;
            selected = minID;
            continue;


        }



    }


}
//######################################################################################################
//######################################################################################################
//######################################################################################################
//######################################################################################################

void BruteForce(Graph *skrydziai, int kelias[],int minkelias[], int startas, int tikslas, int atvykimolaikas, int trukme, int *minimalus){
    int i,ID,reali,pirmaRekursija,atvykimolaikasNaujas;

    if( trukme > *minimalus){ 
      //  printf("--perzengtas minimalus \n");
        return;
    } // Iseinam is rekursijos jei jau perzengem minimalu..

    //printf("Esame virsuneje - %s\n",miestai[startas]);
    //sukuriam kelia TEMP, sekantiem rekursyvam (kad nesimaisytu pointeriai)
    int temp[MAX]; CreateCityIdArray(temp); CopyCityIdArray(temp, kelias);
    AddToCityIdArray(temp, startas);

    //reikalinga laiko apskaiciavimui
    if(atvykimolaikas == -1) pirmaRekursija = 1;
    else pirmaRekursija = 0;
    //

    if (startas  == tikslas){
        // Jei mazesnis uz minimalu
     //   printf("Startas == tikslas\n");
        if( *minimalus > trukme){ // nustatom nauja minimalu ir pakeiciam minimalu kelia
            *minimalus = trukme;
            CopyCityIdArray(minkelias, temp);
        //    printf("Keiciama minimali reiksme %d\n",*minimalus);
        }
        return;    
    }

    if(CountList(skrydziai->cities[startas]) == 0 ){ // Jei nera briaunu IR MES NE TIKSLE 
       // printf("--Saku nera");
        return; // iseinam is rekursijos
    }

    for (i=1; i<=CountList(skrydziai->cities[startas]); i++ ){ // praeinam pro visas virsunes sakas

        //Suskaiciuojam realias trukmes t.y. laukimas + skrydzio trukme
        reali = realitrukmeBrute(atvykimolaikas, 
        GetLaikasList(skrydziai->cities[startas], i) , 
        GetTrukmeList(skrydziai->cities[startas], i),pirmaRekursija) ;

        ID = GetValueList(skrydziai->cities[startas], i);

       // printf("--Is %s i Virsune %s trukme - %d ",miestai[startas],miestai[ID],reali);
        atvykimolaikasNaujas=paroslaikas(GetLaikasList(skrydziai->cities[startas], i) 
                            + GetTrukmeList(skrydziai->cities[startas],i));
       // printf("naujas atvykimo laikas - %d:%d\n",atvykimolaikas/60,atvykimolaikas%60);

        BruteForce(skrydziai, temp, minkelias, ID, tikslas, atvykimolaikasNaujas, trukme+reali, minimalus);
            
    }
}




int main()
{
	//Graph
	Graph *skrydziai = CreateGraph();

// FILE ###############################################
	int valandos, minutes, trukme;
	int i=0; int g; int z[2];

	char pradzia[20], tikslas[20];
	FILE* data=fopen("skrydziai.txt", "r");
	fscanf(data,"// Miestas - Miestas Valandos:Minutes Minutes\n");
    while(fscanf(data, "%s - %s %d:%d %d\n", pradzia, tikslas, 
    		&valandos, &minutes, &trukme) != EOF){


    	//MIESTAS TO INT 
    	if( ThereIsInArray(pradzia,miestai,&g) == 0 ){ 
    		strcpy(miestai[i++], pradzia); z[0] = (i-1);
    		countOfCity++;
    	}
    		else if(ThereIsInArray(pradzia,miestai,&g) == 1) { 
               z[0] = g; 
           } 

        if( ThereIsInArray(tikslas,miestai,&g) == 0 ){ 
        	strcpy(miestai[i++], tikslas); z[1] = (i-1);
        	countOfCity++;
        }
        	else {
                z[1] = g;
            } 
        //##############
        

 		AddFlight(z[0], z[1], (valandos*60+minutes), trukme, &skrydziai ); // konstruojam grafa
    }

    
    

    fclose(data);
// ###################################################   
    printf("\n########Tvarkarastis############\n");
    for(i=0; i<countOfCity; i++ ){
        printf("Skrydziai is %s:\n", miestai[i] );
        if( CountList(skrydziai->cities[i]) == 0 ){
            printf("--Skrydziu nera");
            printf("\n\n");
            continue;
        }  
        for (g=1; g<=CountList(skrydziai->cities[i]); g++ ){
            printf("--%d)%s  Laikas - %d:%.2d   Trukme %d val %d min\n",g,miestai[GetValueList(skrydziai->cities[i], g)],
            (GetLaikasList(skrydziai->cities[i], g))/60,
            (GetLaikasList(skrydziai->cities[i], g))%60,
            (GetTrukmeList(skrydziai->cities[i], g))/60,
            (GetTrukmeList(skrydziai->cities[i], g))%60  );
        }    
        printf("\n");
    }
    printf("################################\n");


// ###################################################
    int ieskot,startas;
    S *s = (S*) malloc(countOfCity * sizeof(S));
    for(i=0;i<countOfCity;i++){
        (s+i)->reiksme = 0x7FFF;
        (s+i)->marked = 0;
    } 

    printf("##########SARASAS###############\n");    
    for(i=0;i<countOfCity;i++){
        printf("%s ID-%d\n",miestai[i],i );
    }    

    printf("Is kokio miesto ieskoti skrydziu?\n");
    scanf("%d",&startas);

    printf("I koki miesta ieskoti skrydziu?\n");
    scanf("%d",&ieskot);

    // ######## EXCEPTION ##########
    if( CountList(skrydziai->cities[startas]) < 1  ){
        printf("Toks skrydis neegzistuoja\n");
        return 1;
    } 

    if( ThereIsInArray(miestai[ieskot], miestai, &g) == 0 ||
        ThereIsInArray(miestai[startas], miestai, &g) == 0
        ){
        printf("Toks skrydis neegzistuoja\n");
        return 1;
    } 
    // #############################
 
    (s+startas)->reiksme = 0; (s+startas)->marked = 1; (s+startas)->atvykimolaikas=-1;

    //****************************************************************************
    ShortestPath(skrydziai, startas, s); 
    //****************************************************************************

    // ######## EXCEPTION ########## 
    if( (s+ieskot)->reiksme == 0x7FFF ){
        printf("Toks skrydis neegzistuoja\n");
        return 1;
    } 
    // #############################


    printf("^^^^^^^^^Marsrutas^^^^^^^^^^^^^^: \n\n");
    printf("##Djikstra algoritmo apskaiciavimas:\n");

    int marsrut[countOfCity]; for(i=0;i<countOfCity;i++) marsrut[i] = -1;
    int pradet,skaitliuk=1;

    i = ieskot;
    marsrut[countOfCity] = ieskot;
    while ( miestai[i] != miestai[startas]){
        marsrut[countOfCity-skaitliuk] = (s+i)->buves;
        i=(s+i)->buves;
        skaitliuk++;
    }
    pradet = countOfCity - skaitliuk;
    for(i=pradet; i<countOfCity; i++){
        printf("%s -> ",miestai[marsrut[i]]);
    }
        printf("%s \n",miestai[ieskot]);
        printf("Skrydzio trukme %d val %d min\n",((s+ieskot)->reiksme)/60,((s+ieskot)->reiksme)%60 );
        printf("Laikas minutem - %d\n",(s+ieskot)->reiksme);
        printf("Atvykimas %d:%d\n",(s+ieskot)->atvykimolaikas/60,(s+ieskot)->atvykimolaikas%60);
        printf("\n");
    
    printf("##BruteForce algoritmo apskaiciavimas:\n");


    int kelias[MAX]; CreateCityIdArray(kelias); 
    int minkelias[MAX]; CreateCityIdArray(minkelias); 
    int minimalitrukme = 0x7FFF; int *minimalus = &minimalitrukme; 

        
    //****************************************************************************
    BruteForce(skrydziai,kelias,minkelias, startas, ieskot, -1 , 0, minimalus);
    //****************************************************************************
        printf(" ");
        for(i=0;i<MAX;i++){
            if( minkelias[i] != -1)
            printf("-> %s ",miestai[minkelias[i]]);
        }
        printf("\n");
        printf("Skrydzio trukme %d val %d min\n",minimalitrukme/60,minimalitrukme%60);
        printf("Laikas minutem - %d\n\n",minimalitrukme);


	
	return 0;
}