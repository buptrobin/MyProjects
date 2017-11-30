import pandas as pd
import matplotlib.pyplot as plt
import numpy as np
import seaborn as sns

def gd(x_start, step, g):
    x = x_start
    for i in range(20):
        grad = g(x)
        x -= grad * step
        print '[ Epoch {0} ] grad = {1}, x = {2}'.format(i, grad, x)
        if abs(grad) < 1e-6:
            break
    return x

def f(x):
    return x * x - 2 * x + 1

def g(x):
    return 2 * x - 2

x = np.linspace(-5,7,100)
y = f(x)
plt.plot(x, y)
plt.show()

gd(5,0.1,f)
