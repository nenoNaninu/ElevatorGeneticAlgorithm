# -*- coding: utf-8 -*-
import json
import matplotlib.pyplot as plt
import numpy as np

path = r"C:\Users\nari_\AppData\Local\ElevatorGA\20190110184321\evalu.json"
file = open(path,encoding="utf-8")
print(file)
json = json.load(file)

print(json)

x = np.arange(0,len(json),1)

plt.plot(x,json)

plt.show()


