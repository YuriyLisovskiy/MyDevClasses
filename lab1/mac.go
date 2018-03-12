package main

import (
	"net"
	"log"
	"fmt"
)

var (
	targetMacAddr = []string {"mac_address_1", "mac_address_2", "mac_address_n"}
)

func getMacAddr() ([]string, error) {
	interfaces, err := net.Interfaces()
	if err != nil {
		return nil, err
	}
	var macAddr []string
	for _, device := range interfaces {
		addr := device.HardwareAddr.String()
		if addr != "" {
			macAddr = append(macAddr, addr)
		}
	}
	return macAddr, nil
}

func checkPermission() bool {
	macAddr, err := getMacAddr()
	if err != nil {
		log.Fatal(err)
	}
	for i, addr := range macAddr {
		if addr == targetMacAddr[i] {
			return true
		}
	}
	return false
}

func main()  {
	if checkPermission() {
		fmt.Print("You can run this app on your maschine.")
	} else {
		fmt.Print("You can't run this app on your masсhine: prmission denied.")
	}

}
