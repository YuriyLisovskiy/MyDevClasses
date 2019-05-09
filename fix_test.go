package main

import (
	"fmt"
	"io/ioutil"
	"os"
	"strings"
)

const (
	TEMPLATE_DIR = "./templates/"

	OLD_TEST_TEMPLATE = TEMPLATE_DIR + "test_old.tmpl"
	NEW_TEST_TEMPLATE = TEMPLATE_DIR + "test_new.tmpl"

	OUT = "../ScientificReport.Test/"

	SERVICES_TESTS_OUT = OUT + "ServicesTests/"
)


func read(path string) (string, error) {
	bytesData, err := ioutil.ReadFile(path)
	return string(bytesData), err
}

func write(path string, data string) error {
	f, err := os.Create(path)
	defer f.Close()
	if err != nil {
		fmt.Println(err)
		return err
	}
	_, err = f.WriteString(data)
	if err != nil {
		fmt.Println(err)
		return err
	}
	return nil
}

func render(entity string, template string) string {
	res := strings.Replace(template, "{{ entity }}", entity, -1)
	res = strings.Replace(res, "{{ entity_var }}", string(strings.ToLower(string(entity[0]))) + entity[1:], -1)
	return res
}

func replace(content, newFragment string) string {
	start := strings.Index(content, "ServiceTests\n\t{") + len("ServiceTests\n\t{")
	end := strings.Index(content, "service.Verify(a => a.Exists(guid));\n\t\t}") + len("service.Verify(a => a.Exists(guid));\n\t\t}")

	result := content[:start] + newFragment + content[end:]

	return strings.Replace(result, "_mockContext", "GetMockContext()", -1)
}

func generate(entityName string) {
	newTestTemplate, err := read(NEW_TEST_TEMPLATE)
	if err != nil {
		fmt.Println(entityName + "ServiceTests NEW: " + err.Error())
		return
	}

	testPath := SERVICES_TESTS_OUT + entityName + "ServiceTests.cs"
	if _, err := os.Stat(testPath); !os.IsNotExist(err) {
		content, err := read(testPath)
		if err != nil {
			fmt.Println(entityName + "ServiceTests CONTENT: " + err.Error())
			return
		}

		result := replace(content, render(entityName, newTestTemplate))

		if _, err := os.Stat(testPath); !os.IsNotExist(err) {
			err = write(testPath, result)
			if err != nil {
				fmt.Println(entityName + "ServiceTests: " + err.Error())
				return
			}
			fmt.Println("'" + testPath + "' fixed")
		}
	}
}

func main() {
	entities := os.Args[1:]
	if len(entities) == 0 {
		fmt.Println("Usage: go run test_fix.go entity1 entity2 ...")
	} else {
		for _, entity := range entities {
			generate(entity)
		}
	}
}
