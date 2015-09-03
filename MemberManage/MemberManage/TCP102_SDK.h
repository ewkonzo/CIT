/********************************************************************************************************************
* @file    : TCP102_SDK.h
* @author  : ChinaTenet
* @version : V1.1
* @date    : 2013.11.25
* @brief   : Head file o TCP102.dll
*********************************************************************************************************************/

/********************************************************************************************************************/
#ifndef _TCP102_H
#define _TCP102_H

#ifdef TCP102_SDK_EXPORTS
#define TCP102_SDK_API __declspec(dllexport)
#else
#define TCP102_SDK_API __declspec(dllimport)
#endif

/********************************************************************************************************************/
#define TCP102_CMD_RESET				0x01 //PC->TCP102
#define TCP102_CMD_READ_VER				0x02 //PC->TCP102
#define TCP102_CMD_DOWNLOAD_CONFIG	    0x03 //PC->TCP102
#define TCP102_CMD_SET_TIME				0x04 //PC->TCP102
#define TCP102_CMD_DOWNLOAD_CARD_LIST	0x05 //PC->TCP102
#define TCP102_CMD_CLEAR_CARD_LIST	    0x06 //PC->TCP102
#define TCP102_CMD_CLEAR_RECORD_LIST	0x07 //PC->TCP102

#define TCP102_CMD_UPLOAD_BAR_CODE      0x1A //TCP102->PC
#define TCP102_CMD_READED_CARD	        0x11 //TCP102->PC
#define TCP102_CMD_READ_IC				0x12 //PC->TCP102
#define TCP102_CMD_READ_IC_DONE	        0x13 //TCP102->PC
#define TCP102_CMD_WRITE_IC				0x14 //PC->TCP102
#define TCP102_CMD_WRITE_IC_DONE	    0x15 //TCP102->PC
#define TCP102_CMD_IC_ENTER	            0x16 //TCP102->PC
#define TCP102_CMD_IC_EXIT	            0x17 //TCP102->PC
#define TCP102_CMD_UPLOAD_ID_RECORD	    0x18 //TCP102->PC

#define TCP102_CMD_RS232_TRANS	        0x19 //PC->TCP102
#define TCP102_CMD_LED_DIS				0x21 //PC->TCP102
#define TCP102_CMD_RS485_TRANS			0x22 //PC->TCP102

#define TCP102_CMD_WRITE_IO				0x41 //PC->TCP102
#define TCP102_CMD_READ_IO				0x42 //PC->TCP102
#define TCP102_CMD_UPLOAD_IO_STATE		0x43 //TCP102->PC

#define TCP102_CMD_LIGHT_CONTROL		0x44 //PC->TCP102
#define TCP102_CMD_BARRIER_CONTROL		0x45 //PC->TCP102
#define TCP102_CMD_RELAY_CONTROL		0x46 //PC->TCP102

#define TCP102_CMD_PLAY_VOICE			0x51 //PC->TCP102
#define TCP102_CMD_PLAY_CHARGE			0x52 //PC->TCP102
#define TCP102_CMD_PLAY_PERIOD			0x53 //PC->TCP102
#define TCP102_CMD_PLAY_PERIOD_DAYS		0x54 //PC->TCP102

/********************************************************************************************************************/
#define TCP102_ERR_OK                   0x00  //OK
#define TCP102_ERR_PARAMETER		    0x01  //Parameter error.
#define TCP102_ERR_WSASTARTUP			0x02  //Can't start Windows Asynchronous Socket.
#define TCP102_ERR_GET_HOST_NAME		0x03  //Can't get host name.
#define TCP102_ERR_GET_SOCKET			0x04  //Acquire socket fail from your system.
#define TCP102_ERR_OPEN_PORT			0x05  //Can't open the port 8008 of your system.Maybe the 8008 port has been opened by other application.
#define TCP102_ERR_LISTEN_PORT			0x06  //Can't listen the port 8008 of your system.
#define TCP102_ERR_START_SERVER	        0x07  //Create server thread fail.This server thread used to listen 8008 port and receive data when TCP102 conrtoller send data to PC automatically.
#define TCP102_ERR_CONNECT_TMO			0x08  //Connect to controller timeout.
#define TCP102_ERR_ACCEPT_ASK		    0x09  //Can't receive Response from controller.The TCP102 should send a Response to PC when it received and execute a command.
#define TCP102_ERR_ASK_CHECK            0x0A  //The Response received from controller Check Error.                         
#define TCP102_ERR_SOCKET_SEND			0x0B  //Send data to TCP102 controller fail.
#define TCP102_ERR_CTR_EXE_CMD_FAIL	    0x0C  //Controller can't execute the command.Maybe parameter of the command wrong.

/********************************************************************************************************************/
//Set time of controller
typedef struct 
{
	unsigned short Year;  //1970~2999
	unsigned char  Month; //1~12	
	unsigned char  Date;  //1~31
	unsigned char  Hour;  //0~24
	unsigned char  Minute;//0~60
	unsigned char  Second;//0~60
}STRUCT_SET_TIME;

//Ask the controller to read a IC card
typedef struct 
{
	unsigned char ReaderNum; //IC card reader of cardbin - 0x03£¬IC card reader of card dispenser - 0x04	
	unsigned char SectorNum; //0-15
	unsigned char Key[6];    //default Key is 0xFFFFFFFFFFFF
}STRUCT_RD_IC;

//Ask the controller to write a IC card
typedef struct 
{
	unsigned char ReaderNum; //IC card reader of cardbin - 0x03£¬IC card reader of card dispenser - 0x04	
	unsigned int  CardID;    //IC card nember
	unsigned char SectorNum; //0-15
	unsigned char Key[6];    //default Key is 0xFFFFFFFFFFFF
	unsigned char Block0Data[16]; //block 0 data
	unsigned char Block1Data[16]; //block 1 data
	unsigned char Block2Data[16]; //block 2 data
}STRUCT_WR_IC;

//Led diaplay
typedef struct 
{
	unsigned char DisDataLen; //length of display buff. Should < 128.
	unsigned char *DisData;   //display buff
}STRUCT_LED_DISPLAY;

//RS485 transmit data
typedef struct 
{
	unsigned char Translen;   //length of transmit data buff.Should < 128.
	unsigned char *TransData; //transmit data buff.
}STRUCT_RS485_TRANS;

//RS232 transmit data
typedef struct
{
	unsigned char ComNumber;      //USART nember. COM1 - 0x01,COM2 - 0x02
	unsigned char TransDataLen;   //length of transmit data buff.Should < 128.
	unsigned char *TransDataBuff; //transmit data buff.
}STRUCT_RS232_TRANS;

//Card dispenser ctrol Command
typedef enum
{
	DISPENSER_RESET     = 0x31,
	DISPENSER_ISSUE_1	= 0x32,
	DISPENSER_ISSUE_2	= 0x33,
	DISPENSER_CALL_BACK	= 0x34
}DISPENSER_CMD;

//Card collector ctrol Command
typedef enum
{
	COLLECTOR_RESET	  = 0x35,
	COLLECTOR_RECEIVE = 0x36,
	COLLECTOR_RECEDE  = 0x37
}COLLECTOR_CMD;

//Read IO state of Controller
typedef struct 
{
	unsigned char IOStateData1;
	unsigned char IOStateData2;
	unsigned char IOStateData3;
	unsigned char IOStateData4;
}STRUCT_IO_STATE;
/******************** IOStateDataX function define ********************************
IOStateData1.0 - Card dispenser or collector state. 0 - card machine error; 1 - normally
IOStateData1.1 - Card dispenser or collector state. 0 - card low,1 - OK
IOStateData1.2 - Card dispenser or collector state. 0 - card is where is read or waiting taking away,1 - card is not in place
IOStateData1.3 - Card dispenser or collector state. 0 - card is ready,1 - card is not ready 
IOStateData1.4 - No used
IOStateData1.5 - No used
IOStateData1.6 - No used
IOStateData1.7 - No used

IOStateData2.0 - Dial switch state. 0-DIP4 ON,1-DIP4 OFF
IOStateData2.1 - Dial switch state. 0-DIP3 ON,1-DIP3 OFF
IOStateData2.2 - Dial switch state. 0-DIP2 ON,1-DIP2 OFF
IOStateData2.3 - Dial switch state. 0-DIP1 ON,1-DIP1 OFF
IOStateData2.4 - Card dispenser or collector state. 0 - card empty,1 - OK
IOStateData2.5 - Dial switch state. 0-DIP5 ON,1-DIP5 OFF
IOStateData2.6 - Dial switch state. 0-DIP6 ON,1-DIP6 OFF
IOStateData2.7 - Dial switch state. 0-DIP7 ON,1-DIP7 OFF

IOStateData3.0 - No used
IOStateData3.1 - Help botton state. 0 - botton press down
IOStateData3.2 - Take card botton state. 0 - botton press down
IOStateData3.3 - 0-loop detector of barrier gate is effective, 1-ineffective 
IOStateData3.4 - Dial switch state. 0-DIP8 ON,1-DIP8 OFF
IOStateData3.5 - Optocoupler 1 input signal. 0 - ineffective,1 - effective
IOStateData3.6 - Optocoupler 2 input signal. 0 - ineffective,1 - effective
IOStateData3.7 - No used

IOStateData4.0 - No used
IOStateData4.1 - No used
IOStateData4.2 - No used
IOStateData4.3 - No used
IOStateData4.4 - No used
IOStateData4.5 - No used
IOStateData4.6 - No used
IOStateData4.7 - No used

**********************************************************************************/

//Light control command
typedef enum
{
	LIGHT_OPEN  = 0x01,
	LIGHT_CLOSE = 0x00,
}LIGHT_CMD;

//Barrier control command
typedef enum
{
	BARRIER_CLOSE = 0x00,
	BARRIER_OPEN  = 0x01,
	BARRIER_STOP  = 0x02,
}BARRIER_CMD;

//Relay control command
typedef enum
{
	RELAY1_CLOSE = 0x10,
	RELAY1_OPEN  = 0x11,
	RELAY2_CLOSE = 0x20,
	RELAY2_OPEN  = 0x21,
	RELAY3_CLOSE = 0x30,
	RELAY3_OPEN  = 0x31,
	RELAY4_CLOSE = 0x40,
	RELAY4_OPEN  = 0x41,
}RELAY_CMD;

//Ask the controller to paly voices
typedef struct 
{
	unsigned char VoiceNum;      //number of voices
	unsigned char VoiceBuff[32]; //table of voices want to play
}STRUCT_PLAY_VOICE;

//card type
typedef enum
{
	CAR_T_MOTORCYCLE = 0x01, 
	CAR_T_SMALL      = 0x02,
	CAR_T_MIDDLE     = 0x03,
	CAR_T_BIG        = 0x04,
	CAR_T_SUPER      = 0x05,
	CAR_T_RESERVED   = 0x06,
}CAR_TYPE;

//Ask the controller to paly charge
typedef struct 
{
	unsigned char CarType; //CAR_TYPE - 0x01~0x06
	unsigned short Charge;
}STRUCT_PLAY_CHARGE;

//Ask the controller to play effective period
typedef struct 
{
	unsigned short Year; //1970~2999
	unsigned char Month; //1~12
	unsigned char Day;   //1~31
}STRUCT_PLAY_PERIOD;

//Download card list to controller
typedef struct 
{
	unsigned short Sequence; //every card list has only a Sequence 
	unsigned int   CardID;   //decimalism card number
	unsigned char  CardType; //CAR_TYPE - 0x01~0x06
	unsigned short StopEffectTimeYear; 
	unsigned char  StopEffectTimeMonth;
	unsigned char  StopEffectTimeDay;
	unsigned char  PlateNo[12];
}STRUCT_CARD_LIST;

/*************************************************************************************************/
//controller readed a IC/ID card
typedef struct _READED_CARD_MSG
{
	unsigned char ControllerIP[4];//IP of controller
	unsigned char CommandSeq;//command sequence. It will add 1 after send a command .
	unsigned char ReaderID;//ID card reader of card dispenser - 0x01£¬ ID card reader of cardbin - 0x02
	                       //IC card reader of cardbin - 0x03£¬IC card reader of card dispenser - 0x04
	unsigned char CardID[4];//card number.The low byte in the front
}CARD_NUMBER_MSG;
typedef void(CALLBACK *ReadedCardNumber)(DWORD dwUser,CARD_NUMBER_MSG *p_card_msg);

//The return result when you call TCP102_ReadICCard or TCP102_WriteICCard
typedef enum
{
	IC_READER_STATE_OK = 0x00,
	IC_READER_STATE_CHECK = 0x01,
	IC_READER_STATE_ARG = 0x02,
	IC_READER_STATE_RD_ERR = 0x11,
	IC_READER_STATE_WR_ERR = 0x12,
	IC_READER_STATE_KEY_ERR = 0x13,
}IC_READER_STATE;

//In view of a long delay time is needed to read IC card; the read data will not  
//return immediately when you call function TCP102_ReadICCard. 
//TCP102 controllor will send datas of IC card to PC automatically after for a while.
//This callback function will be called when received IC card data.
typedef struct _IC_CARD_MSG
{
	unsigned char ControllerIP[4];
	unsigned char CommandSeq;
	unsigned char ReaderID;//IC card reader of cardbin - 0x03£¬IC card reader of card dispenser - 0x04	
	unsigned char ReadState; //Reference IC_READER_STATE
	unsigned char CardID[4];//card number.The low byte in the front
	unsigned char SectorNum;
	unsigned char SectorData[48];
}IC_CARD_MSG;
typedef void(CALLBACK *ReadedICCard)(DWORD dwUser,IC_CARD_MSG *p_ic_card_msg);


//This callback function will be called when received the result of TCP102_WriteICCard.
typedef struct _WRITE_IC_RETURN
{
	unsigned char ControllerIP[4];
	unsigned char CommandSeq;
	unsigned char ReaderID;//IC card reader of cardbin - 0x03£¬IC card reader of card dispenser - 0x04
	unsigned char ReadState;//Reference IC_READER_STATE
	unsigned char CardID[4];//card number.The low byte in the front
	unsigned char SectorNum;
}WRITE_IC_RETURN;
typedef void(CALLBACK *WriteICReturn)(DWORD dwUser,WRITE_IC_RETURN *p_write_ic_return);

//card type
typedef enum
{
	VIP_CARD  = 0x01,  
	MONTH_CARD,
	MONEY_CARD,
	TEMP_CARD
}CARD_TYPE;

//This callback function will be called when a IC card enter park.
typedef struct _ENTERED_IC_CARD_MSG
{
	unsigned char ControllerIP[4];
	unsigned char CommandSeq;
	unsigned char ReaderID;
	unsigned char CardID[4]; //card number.The low byte in the front
	unsigned char CardType; //Reference CARD_TYPE
	unsigned char EnterTime[7]; //EnterTime[0-1]:Year,The low byte in the front, Year = (EnterTime[1]<<8)|EnterTime[0]
								//EnterTime[2]:month, 
								//EnterTime[3]:day, 
								//EnterTime[4]:hour, 
								//EnterTime[5]:minute, 
								//EnterTime[6]:second
}ENTERED_IC_CARD_MSG;
typedef void(CALLBACK *EnteredICCard)(DWORD dwUser,ENTERED_IC_CARD_MSG *p_entered_ic_msg);

//This callback function will be called when a IC card exit park.
typedef struct _EXITED_IC_CARD_MSG
{
	unsigned char ControllerIP[4];
	unsigned char CommandSeq;
	unsigned char ReaderID;
	unsigned char CardID[4];//card number.The low byte in the front
	unsigned char CardType;//Reference CARD_TYPE
	unsigned char EnterTime[7];
	unsigned char ExitTime[7];
}EXITED_IC_CARD_MSG;
typedef void(CALLBACK *ExitedICCard)(DWORD dwUser,EXITED_IC_CARD_MSG *p_exited_ic_msg);

//Ctroller upload ID card record
typedef struct _ID_CARD_RECORD_MSG
{
	unsigned char ControllerIP[4];
	unsigned char CommandSeq;
	unsigned short Seq_Num;
	unsigned char CardID[4];
	unsigned char CardType;
	unsigned char EnterTime[7];
}ID_CARD_RECORD_MSG;
typedef void(CALLBACK *CtrUploadIDRecord)(DWORD dwUser,ID_CARD_RECORD_MSG *p_id_record_msg);

//Ctroller upload barcode
typedef struct _BAR_CODE_MSG
{
	unsigned char ControllerIP[4];
	unsigned char CommandSeq;
	unsigned char DataLen;
	unsigned char BarData[128];
}BAR_CODE_MSG;
typedef void(CALLBACK *CtrUploadBarCode)(DWORD dwUser,BAR_CODE_MSG *p_bar_code_msg);

//Ctroller upload IO state
typedef struct _IO_STATE_MSG
{
	unsigned char ControllerIP[4];
	unsigned char CommandSeq;
	unsigned char IO_Data[4];
	unsigned char ChangeBitBytes[4];
}IO_STATE_MSG;
typedef void(CALLBACK *CtrUploadIOState)(DWORD dwUser,IO_STATE_MSG *p_io_state_msg);

//struct of callback functions
typedef struct _STRUCT_CALLBACK_FUNCTIONS
{
	ReadedCardNumber p_ReadedCardNumberCallBack;
	ReadedICCard p_ReadedICCardCallBack;
	WriteICReturn p_WriteICReturnCallBack;
	EnteredICCard p_EnterICCardCallBack;
	ExitedICCard p_ExitedICCardCallBack;
	CtrUploadIDRecord p_CtrUploadIDRecordCallBack;
	CtrUploadBarCode p_CtrUploadBarCodeCallBack; 
	CtrUploadIOState p_CtrUploadIOStateCallBack;
}STRUCT_CALLBACK_FUNCTIONS;

/********************************************************************************************************************/
TCP102_SDK_API unsigned char __stdcall TCP102_InitDll(DWORD dwUser);
TCP102_SDK_API unsigned char __stdcall TCP102_GetDllVersion(unsigned char *version);
TCP102_SDK_API unsigned char __stdcall TCP102_InitCallBack(DWORD dwUser,STRUCT_CALLBACK_FUNCTIONS *p_call_back);
TCP102_SDK_API unsigned char __stdcall TCP102_ResetController(char *ctr_ip,unsigned char sequence,unsigned char time_out);
TCP102_SDK_API unsigned char __stdcall TCP102_ReadControllerVersion(char *ctr_ip,unsigned char sequence,unsigned char time_out,unsigned char *ver_data);
TCP102_SDK_API unsigned char __stdcall TCP102_SetTime(char *ctr_ip,unsigned char sequence,unsigned char time_out,STRUCT_SET_TIME *time);
TCP102_SDK_API unsigned char __stdcall TCP102_ReadICCard(char *ctr_ip,unsigned char sequence,unsigned char time_out,STRUCT_RD_IC *struct_rd_ic);
TCP102_SDK_API unsigned char __stdcall TCP102_WriteICCard(char *ctr_ip,unsigned char sequence,unsigned char time_out,STRUCT_WR_IC *struct_wr_ic);
TCP102_SDK_API unsigned char __stdcall TCP102_LedDisplay(char *ctr_ip,unsigned char sequence,unsigned char time_out,STRUCT_LED_DISPLAY *led_dis);
TCP102_SDK_API unsigned char __stdcall TCP102_RS485Transmit(char *ctr_ip,unsigned char sequence,unsigned char time_out,STRUCT_RS485_TRANS *trans);
TCP102_SDK_API unsigned char __stdcall TCP102_RS232Transmit(char *ctr_ip,unsigned char sequence,unsigned char time_out,STRUCT_RS232_TRANS *rs232_trans);
TCP102_SDK_API unsigned char __stdcall TCP102_CardDispenserControl(char *ctr_ip,unsigned char sequence,unsigned char time_out,DISPENSER_CMD cmd);
TCP102_SDK_API unsigned char __stdcall TCP102_CardCollectorControl(char *ctr_ip,unsigned char sequence,unsigned char time_out,COLLECTOR_CMD cmd);
TCP102_SDK_API unsigned char __stdcall TCP102_WriteIO(char *ctr_ip,unsigned char sequence,unsigned char time_out,unsigned char io_data);
TCP102_SDK_API unsigned char __stdcall TCP102_ReadIO(char *ctr_ip,unsigned char sequence,unsigned char time_out,STRUCT_IO_STATE *io_state);
TCP102_SDK_API unsigned char __stdcall TCP102_LightControl(char *ctr_ip,unsigned char sequence,unsigned char time_out,LIGHT_CMD cmd);
TCP102_SDK_API unsigned char __stdcall TCP102_BarrierControl(char *ctr_ip,unsigned char sequence,unsigned char time_out,BARRIER_CMD cmd);
TCP102_SDK_API unsigned char __stdcall TCP102_RelayControl(char *ctr_ip,unsigned char sequence,unsigned char time_out,RELAY_CMD cmd);
TCP102_SDK_API unsigned char __stdcall TCP102_PlayVoice(char *ctr_ip,unsigned char sequence,unsigned char time_out,STRUCT_PLAY_VOICE *voice);
TCP102_SDK_API unsigned char __stdcall TCP102_PlayCharge(char *ctr_ip,unsigned char sequence,unsigned char time_out,STRUCT_PLAY_CHARGE *charge);
TCP102_SDK_API unsigned char __stdcall TCP102_PlayPeriod(char *ctr_ip,unsigned char sequence,unsigned char time_out,STRUCT_PLAY_PERIOD *period);
TCP102_SDK_API unsigned char __stdcall TCP102_PlayPeriodDays(char *ctr_ip,unsigned char sequence,unsigned char time_out,unsigned short period_days);
TCP102_SDK_API unsigned char __stdcall TCP102_LoadCardList(char *ctr_ip,unsigned char sequence,unsigned char time_out,STRUCT_CARD_LIST *card_list);
TCP102_SDK_API unsigned char __stdcall TCP102_ClearCardList(char *ctr_ip,unsigned char sequence,unsigned char time_out);
TCP102_SDK_API unsigned char __stdcall TCP102_ClearRecordList(char *ctr_ip,unsigned char sequence,unsigned char time_out);
TCP102_SDK_API unsigned char __stdcall TCP102_DllExit(void);

#endif
/*********************************************************** END OF DILE *****************************************************************/


