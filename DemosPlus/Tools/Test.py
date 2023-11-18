import json

class Test:
    a = 1
    b = 2
    c = {}

t = Test()
t.a = 234
t.b = 543
t.c[1] = 5

t2 = Test()
t2.c = {}

print(t.c)