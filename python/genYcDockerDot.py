import os
import re

def travelDir(rootdir, fout, nodeset):
	dir_list = os.walk(rootdir)
	edgelist = []
	for root,dirs,files in dir_list:
		for d in dirs:
			# print os.path.join(root,d)
			pass
		for f in files:
			if (f == 'Dockerfile') :
				fname = os.path.join(root,f)
				fullname = fname
				if (fname.find(rootdir + "docker") >= 0):
					continue

				fname = fname.replace(rootdir,'')
				fname = fname.replace('src/main/docker/','')
				fname = fname.replace('-','_')

				layer = fname.split('/')

				fo = open(fullname,'r')
				lines = fo.readlines()
				
				for line in lines:
					if (line.startswith('FROM')):
						dkTo = clean(layer[-2])
						dkFrom = clean(line[5:].replace('docker.yosemitecloud.com/',''))
						nodeset.add(dkFrom)
						nodeset.add(dkTo)
						edgelist.append(dkFrom + '->' + dkTo + ';\n\r')

						print fullname,dkFrom,dkTo

	for node in nodeset:
		fout.write('\t' + node + ';\n\r')

	fout.write('\n\r\n\r')

	for outline in edgelist:
		fout.write('\t' + outline)

def clean(s):
	return s.replace('-','_').replace('/','_').replace('.','_').replace(':','_').strip()


nodeset = set()
rootdir = "/home/robin/yc/etc/docker/"
fout = open("/home/robin/tmp/out.dot","w")
header = '''
digraph Docker {
	node [shape="ellipse"];
	edge [style="solid"];
'''

fout.write(header)

travelDir(rootdir, fout, nodeset)

fout.write('}')

fout.close()
