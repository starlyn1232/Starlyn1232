#include<iostream>
#include<conio.h>
using namespace std;

int main(){
	
	MAIN:
	system("cls");
	
	int read=0, year=0,month=0,week=0,days=0,b1=1,b2=0,temp=0,aux=1;
	float yD=0,mD=0,wD=0,dD=0,hD=0,minD=0,secD=0;
	float aux2;
	
	system("mode con cols=48 lines=24");
	system("color 0f && title Day's processor v1.0 Starlyn1232");
	
	cout << "\nHow many days we'll process?: ";
	cin >> read;
	aux2=read;
	
	yD = aux2/365;
	mD = aux2/12;
	wD = aux2/7;
	dD = aux2;
	hD = 24;
	hD *= aux2;
	minD = 1440;
	minD *= aux2;
	secD = 86400;
	secD *= aux2;
	
	
	if(read>=365){
		aux=1;
		while(read>=365){
			if(b1%2==0){
				b2++;
				if(b2==2){
					read-=1;
					b2=0;
				}
			}
			b1++;
			aux++;
			if(read>=365){
				read-=365;
				year++;
			}
			else{
				break;
			}
		}
	}
	
	if(read>28){
		aux=1;
		int ticked=0;
		while(read>=28){
			if((read<=30)&&(aux==1)){
				week+=4;
				read-=28;
				break;
			}
			if(aux==2){
				read-=28;
				aux++;
				month++;
			}
			
			if((aux%2!=0)&&(aux!=2)&&(read>=31)){
				read-=31;
				aux++;
				month++;
				if(aux==8){
					read-=31;
					month++;
					aux++;
					while(read>=31){
						if((aux%2!=0)&&(aux!=2)&&(read>=31)){
							read-=31;
							aux++;
							month++;
						}
						if((aux%2==0)&&(aux!=2)&&(read>=30)){
							read-=30;
							aux++;
							month++;
						}
					}
					read++;
					break;
				}
			}
			if((aux%2==0)&&(aux!=2)&&(read>=30)){
				read-=30;
				aux++;
				month++;
			}
				if((aux==7)&&(ticked==0)){
					ticked=1;
					aux==7;
				}
		}
	}
	
	if(read>=7){
		while(read>=7){
			week++;
			read-=7;
		}
	}
	if(read>=1){
		while(read>=1){
			days++;
			read-=1;
		}
	}
	
	cout << "\nGeneral Info: " << endl;
	
	if(year!=0){
		cout << "\nYears: " << year;
	}
	if(month!=0){
		cout << "\nMonth: " << month;
	}
	
	if(week!=0){
		cout << "\nWeek: " << week;
	}
	if(days!=0){
		cout << "\nDays: " << days;
	}
	
	cout << "\n\nEach Info: " << endl;
	
	if(year!=0){
		cout << "\nYears: " << yD;
	}
	if(month!=0){
		cout << "\nMonth: " << mD;
	}
	
	if(week!=0){
		cout << "\nWeek: " << wD;
	}
	if(days!=0){
		cout << "\nDays: " << dD;
	}
	if(aux2!=0){
		if(days==0){
			cout << "\nDays: " << dD;
		}
		cout << "\nHours: " << hD;
		cout << "\nMinutes: " << minD;
		cout << "\nSeconds: " << secD;
	}
	
	cout << "\n\n---------------------";
	cout << "\nPress Enter to restart.";
	getch();
	goto MAIN;
	
	return 0;
}
