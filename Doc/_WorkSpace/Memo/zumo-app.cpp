#include <kernel.h>
#include "kernel_cfg.h"
#include "app.h"
#include "mbed.h"
// #include "EthernetInterface.h"
#include "app_config.h"		// WiFI接続時のSSID、key、自分のIPアドレスを設定するファイル
#include "GR_PEACH_WlanBP3595.h"

Serial pc(USBTX, USBRX);

static void _wlan_inf_callback(uint8_t ucType, uint16_t usWid, uint16_t usSize, uint8_t *pucData);
static void error_wait(int ret, const char* str);

GR_PEACH_WlanBP3595 wlan;
DigitalOut red_led(LED1);
DigitalOut green_led(LED3);

int ret;
#define remoteIP_ADDRESS			("192.168.11.4")	/* 相手のIP address */
const int port = 11111;									//　接続相手のサーバ/PCのポート番号

// WiFi Routerとの接続
void WiFi_init() {
	wlan.setWlanCbFunction(_wlan_inf_callback);

	pc.printf("\r\ninitializing\r\n");
	ret = wlan.init(IP_ADDRESS, SUBNET_MASK, DEFAULT_GATEWAY);
	error_wait(ret, "init");	// if error, wait
	pc.printf("My addr=%s\r\n",IP_ADDRESS);

	pc.printf("wlan connecting\r\n");
	ret = wlan.connect(WLAN_SSID, WLAN_PSK);
	error_wait(ret, "wifi connect error");	// if error, message&Red LED turn on
	pc.printf("wlan connectted SSID=%s\r\n",WLAN_SSID);		// if no error , print this message
//　　wifi routerとの接続が成功するとZUMOの青色LEDが点灯する
}
static void error_wait(int ret, const char* str) {
	if (ret != 0) {
		pc.printf(str);
		/* error */
		red_led = 1;
		while (1) {
			Thread::wait(1000);
		}
	}
}
static void _wlan_inf_callback(uint8_t ucType, uint16_t usWid, uint16_t usSize,
		uint8_t *pucData) {
	if (ucType == 'I') {
		if (usWid == 0x0005) {    // WID_STATUS
			if (pucData[0] == 0x01) {     // CONNECTED
				green_led = 1;
			} else {
				green_led = 0;
			}
		}
	}
}
void task_main(intptr_t exinf) {
    char in_buffer[20];
    Endpoint nist;
    UDPSocket sock;

    pc.baud(9600);

	// wifi initialize
	WiFi_init();
	pc.printf("WiFi init end\r\n");

    sock.bind(port);
    pc.printf("UDP Socket bind port=%d\r\n",port);

    nist.set_address(remoteIP_ADDRESS, port);
    pc.printf("endpoint ip=%s　port=%s\r\n",nist.get_address(),nist.get_port());
    char out_buffer[20];	// send buffer
    char sdata[] = "I am GR-PEACH";	// send messge
    int count = 5;
    while (count > 0) {
    	sprintf(out_buffer,"%s %d",sdata,count);
    	sock.sendTo(nist, out_buffer, strlen(out_buffer));
    	pc.printf("send data = %s\r\n",out_buffer);

    	int n = sock.receiveFrom(nist, in_buffer, sizeof(in_buffer));
//    	unsigned int timeRes = ntohl( *((unsigned int*)in_buffer));
    	pc.printf("Rcv %d bytes from %s port %d: \r\n", n, nist.get_address(), nist.get_port());
    	pc.printf("Rcv data=%s\r\n",in_buffer);
    	count--;
    }
//    sock.close();
    while(1) {}
}
