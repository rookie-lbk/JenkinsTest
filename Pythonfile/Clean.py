# -*- coding: UTF-8 -*-
import os
import json

import sys
import importlib
importlib.reload(sys)

# sys.setdefaultencoding('utf8')
currentDir = os.path.split(os.path.realpath(__file__))[0]

def OnStart():
	os.chdir(currentDir + "/../")
	command = "git clean -d -f"
	os.system(command)
	command = "git reset --hard"
	os.system(command)
try:
	OnStart()
except Exception as e:
	print(e)
else:
	pass
finally:
	pass

