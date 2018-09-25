#include <EtherCard.h>
#include <IPAddress.h>

int  m_dir [8];
int  m_pwm [7];
int i = 0;
String data = "";
char temp;

char sensor_data_c[20];
String In_Data;
String  sensor_data_s;
static byte myip[] = { 192,168,0,200 }; // laptop's local Ip
static byte gwip[] = { 192,168,0,1 }; // Lapttop's gatway Ip

// ethernet mac address - must be unique on your network
static byte mymac[] = { 0x1A,0x2B,0x3C,0x4D,0x5E,0x6F }; 

byte Ethernet::buffer[700]; // tcp/ip send and receive buffer
uint8_t ipDestinationAddress[4];

const int dstPort PROGMEM = 1337; // SPI module's port
const int srcPort PROGMEM = 1; // Laptop's port
int  old = 0 , timer = 0;

void udpSerialPrint(uint16_t dest_port, uint8_t src_ip[IP_LEN], uint16_t src_port, const char *dataa, uint16_t len)
{
  IPAddress src(src_ip[0],src_ip[1],src_ip[2],src_ip[3]);
  In_Data = daata;
  if (In_Data.substring(strlen(In_data),strlen(In_data)) != 'a')
      data += In_Data.substring(strlen(In_data),strlen(In_data));
    else if (In_Data.substring(strlen(In_data),strlen(In_data)) == 'a') {
      for(i = 0 ; i < numberOfMotorsAndArms; i++)
      m_dir[i] = data.substring(data.indexOf(('A' + i)) + 1, data.indexOf(('A') + i + 1)).toInt();
      for(i = 0 ; i < numberOfMotorsAndArms - 1; i++)
      m_pwm[i] = data.substring(data.indexOf(('I' + i)) + 1, data.indexOf(('P') + i + 1)).toInt();
      for (i = 0; i < numberOfMotorsAndArms; i++)
        Serial.println(m_dir[i]);
      for (i = 0; i < numberOfMotorsAndArms - 1; i++)
        Serial.println(m_pwm[i]);
      data = "";
    } 
}


void setup () 
{
  Serial.begin(115200);
  //Serial1.begin(9600);

    if (ether.begin(sizeof Ethernet::buffer, mymac) == 0)
    Serial.println(F("Failed to access Ethernet controller"));

  ether.staticSetup(myip, gwip,0,0);
  ether.parseIp(ipDestinationAddress, "192.168.0.201");
  ether.udpServerListenOnPort(&udpSerialPrint, 1337);
  
  
} 

void loop () 
{  
      ether.packetLoop(ether.packetReceive());
    
  
  // concaitnation of the data to be sent (string format)
  // convert the string to ((array of chars))using toCharArray() function 
   timer = millis();
   if (timer - old > 100 )
   {
    
    ether.sendUdp(char test , sizeof(char), dstPort, ipDestinationAddress ,srcPort  ); 
      old = timer;
      
   }
}
