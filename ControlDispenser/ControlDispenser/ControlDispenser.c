/*
* ParkingController.c
*
* Created: 17/9/2556 15:30:12
*  Author: winet
*/

#include <avr/io.h>         // AVR device-specific IO definitions
#include <avr/interrupt.h>	// Interrupt Service routine
#include <compat/deprecated.h>  // Use sbi(), cbi() function
#include <util/delay.h>     // util_delay

#define F_CPU 8000000UL     // CPU clock frequency (in Hertz)

unsigned char AESout[16];
unsigned char RTX[18];

unsigned char i, n;
int data_ok, data_connect;

void USART_Transmit( unsigned char data );
static void delayms(unsigned int intSec);
static void USART_Init(unsigned char baud);


int main(void) {
	DDRC = 0x3F;  //111111
	PORTC = 0x0E; //001110
	DDRD =  0b00000000;
	PORTD = 0b11111100;
	//ADCSRA = 0x00;
	USART_Init(25);
	uint8_t data;
	data_ok = 0;
	data_connect = 0;
	int a = 0;
	int b = 0;
	sbi(PORTC,0);
	//Turn on Computer
	sbi(PORTC,5);
	delayms(2000);
	cbi(PORTC,5);
	
	
	delayms(8000);
	
	while (1)
	{
		sbi(PORTC,0);
		delayms(200);
		cbi(PORTC,0);
		delayms(1200);
		
		
		data = 0b00000000;
		if (bit_is_clear(PIND,2)){//Empty
			data |= 0b00000001;
		}
		
		if (bit_is_clear(PIND,3)){//Low
			data |= 0b00000010;
		}
		if (bit_is_clear(PIND,4)){//Read OK
			data |= 0b00000100;
			}else{
			cbi(PORTC,3);
			delayms(200);
			sbi(PORTC,3);
		}
		
		if (bit_is_clear(PIND,5)){//Green Btn
			data |= 0b00001000;
		}
		if (bit_is_clear(PIND,6)){//Red Btn
			data |= 0b00010000;
		}
		
		if (bit_is_clear(PIND,7)){//Loop
			data |= 0b00100000;
		}
		
		if(data_ok == 1){
			if(bit_is_clear(PIND,5)){
				if(bit_is_clear(PIND,7)){
					if(bit_is_clear(PIND,4)){
						cbi(PORTC,3);
						delayms(200);
						sbi(PORTC,3);
						data_ok = 0;
						data |= 0b01000000;
					}
				}
			}
		}
		
		if(data_connect == 1){
			data |= 0b10000000;
			data_connect = 0;
		}
		
		a = data;
		if(a != b){
			USART_Transmit(data);
			b = a;
		}
		
	}
}


void USART_Transmit( unsigned char data )
{
	/* Wait for empty transmit buffer */
	while ( !( UCSRA & (1<<UDRE)) );
	/* Put data into buffer, sends the data */
	UDR = data;
}

static void USART_Init(unsigned char baud)
{
	// Set baud rate
	UBRRH = baud>>8;
	UBRRL = baud;
	// Enable receiver and tramsmitter
	UCSRB = (1<<RXEN)|(1<<TXEN);
	// Enable Interrupt
	UCSRB |= (1<<RXCIE)|(1<<TXCIE);
	// Set I-bit global interrupt enable
	sei();
	// Set frame format: 8data, NoneParity, 1stop bit
	UCSRC = (1<<URSEL)|(3<<UCSZ0);
}


ISR (USART_RXC_vect)
{
	unsigned char brx = UDR;
	RTX[n] = brx;
	n++;
	if (brx == 13) {
		
		if (RTX[0] == 'S'){//Connect Check
			data_connect = 1;
		}
		
		if (RTX[0] == 'R'){//Ready
			data_ok = 1;
		}
		
		if (RTX[0] == 'W'){//Wait
			delayms(3000);
		}
		
		if (RTX[0] == 'L'){
			//USART_Transmit(RTX[1]);
			//USART_Transmit(13);
			if (RTX[1] == 'I'){
				sbi(PORTC,5);
				delayms(1000);
				cbi(PORTC,5);
			}
			else
			if (RTX[1] == 'O')
			{
				sbi(PORTC,4);
				delayms(1000);
				cbi(PORTC,4);
			}
		}
		
		if (RTX[0] == 'C'){
			//PINC 3 = Dispenser 4 = PayOut
			//PINC 2 = Dispenser 5 = Reset
			//PINC 1 = Dispenser 8 = Callback
			//USART_Transmit(RTX[1]);
			//USART_Transmit(13);
			if (RTX[1] == 'P'){
				cbi(PORTC,3);
				delayms(80);
				sbi(PORTC,3);
			}
			else
			if (RTX[1] == 'R')
			{
				cbi(PORTC,2);
				delayms(80);
				sbi(PORTC,2);
				cbi(PORTC,3);
				delayms(80);
				sbi(PORTC,3);
				delayms(300);
			}
			else
			if (RTX[1] == 'B')
			{
				cbi(PORTC,1);
				delayms(80);
				sbi(PORTC,1);
				cbi(PORTC,3);
				delayms(80);
				sbi(PORTC,3);
				delayms(4000);
			}
		}
		
		n = 0;
		

	}
	return;
}

ISR (USART_TXC_vect)
{
	return;
}

void delayms(unsigned int intSec)
{
	for(;intSec>0;intSec--)
	_delay_ms(4);
}


