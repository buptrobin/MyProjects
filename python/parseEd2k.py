import urllib2
import re

# url='http://www.lbldy.com/video/29406.html'

# resp = urllib2.urlopen(url)
# page = resp.readlines()

s = 'href="ed2k://|fileffdjsa;f dsbknh2xpd63f42oaea5wdjgcqjegdmw|/342423432">'

p = re.compile(r'ed2k.*|/')

match = p.match('ed2k(.)*|/')

if match:
	print match.group()

# print p.findall(s)

# for line in page:
# 	print line
# 	ret = pattern.findall(line);
	# if ret:
		# print ret
	# print ret
	# if 'ed2k' in line:
	# 	print line
	# 	print '----------------------------'
