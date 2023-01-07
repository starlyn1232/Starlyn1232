#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <conio.h>

#define CORRECT "hola"

void ReadPassword(void *data,const char SHOWING_CHAR){
	//Declare temporal variables
	char c = 'a';
	int sz = 0;
	
	do{
		//Wait for keyboard
		c = getch();
		
		//Insert only non-return values
		if (c != '\r' && c != '\b'){
      		//Show allowed char for hidden characters
			printf("%c",SHOWING_CHAR);
			*((char *)data+sz) = c;
			sz++;
		}
		
		if (c == '\b' && sz > 0){
			//"Print" the backspace + space, then remove back that last space with backspace
			printf("\b \b");
			sz--;
			continue;
		}
		//Check non-return values
	}while(c != '\r');
	
	//Set null value
	*(((char*)data)+(sz)) = '\0';
}

int main(){
	
	//Function pointer to use code block memory location
	void (*ptr)(void *,char);
	ptr = ReadPassword;
	
	//Char pointer for our word (Memory allocation used)
	char *word = ((char *)malloc(sizeof(char *)));	
	
	//Read input (Hiddem)
	printf("Input the password:");
	//Call password reader function (Using delegate simulation)
	ptr(word,'*');
	
	//Check password
	if (strcmp(word,CORRECT) != 0){
		//Error msg with alert escape
		fprintf(stderr,"\n\aIncorrect password!",word);
		exit(EXIT_FAILURE);
	}
	
	//Free malloc after granted
	free(word);
	
	//Correct password message
	printf("\nAccess granted!");
	exit(EXIT_SUCCESS);
	
	return 0;
}
