#!/usr/bin/python
import os
import sys
def printIndent(indent):
	for i in range(0, indent):
		sys.stdout.write('\t')

def indentLines(text):
	indent = 0
	for letter in text:
		if(letter == '['):
			sys.stdout.write('[\n')
			indent = indent + 1
			printIndent(indent)
		elif(letter == ']'):
			sys.stdout.write('\n')
			indent = indent - 1
			printIndent(indent)
			sys.stdout.write(']')
		elif(letter == ','):
			sys.stdout.write(letter)
			sys.stdout.write('\n')
			printIndent(indent)
		else:
			sys.stdout.write(letter)
	sys.stdout.write('\n')


if(len(sys.argv) < 2):
	exit()

filepath = sys.argv[1]

if not os.path.isfile(filepath):
	print('file not exist.')
	exit

f = open(filepath)
for line in f.readlines():
	indentLines(line)




