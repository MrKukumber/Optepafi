
def main():
    objects = ["rozbity povrh",
               "velmi rozbity povrch",
               "balvanove pole",
               "huste balvanove pole",
               "kamenity povrch",
               "nepriechodny mociar",
               "mociare",
               "vegetacia dobra viditelnost",
               "otvorena krajina",
               "drsna otvorena krajina",
               "les",
               "vegetacia"]
    combs = [("balvanove pole", "rozbity povrch"), ("balvanove pole","velmi rozibty povrch"),
             ("kamenity povrch","rozbity povrch"), ("kamenity povrch","balvanove pole"),
             ("mociare","rozbity povrch"), ("mociare","balvanove pole"), ("mociare","huste balvanove pole"), ("mociare","kamenity povrch"),
             ("vegetacia dobra viditelnost", "rozbity povrch"), ("vegetacia dobra viditelnost", "velmi rozbity povrch"), ("vegetacia dobra viditelnost", "balvanove pole"), ("vegetacia dobra viditelnost", "kamenity povrch"), ("vegetacia dobra viditelnost", "mociare"),
             ("otvorena krajina","rozbity povrch"), ("otvorena krajina","balvanove pole"), ("otvorena krajina","mociar"),
             ("drsna otvorena krajina","rozbity povrch"), ("drsna otvorena krajina","velmi rozbity povrch"), ("drsna otvorena krajina","balvanove pole"), ("drsna otvorena krajina","huste balvanove pole"), ("drsna otvorena krajina","kamenity povrch"), ("drsna otvorena krajina","nepriechodny mociar"), ("drsna otvorena krajina","mociare"), ("drsna otvorena krajina", "vegetacia dobra viditelnost"),
             ("les","rozbity povrch"), ("les","velmi rozbity povrch"), ("les","balvanove pole"), ("les","huste balvanove pole"), ("les","kamenity povrch"), ("les","nepriechodny mociar"), ("les","mociare"), ("les","vegetacia dobra viditelnost"),
             ("vegetacia","rozbity povrch"), ("vegetacia","velmi rozbity povrch"), ("vegetacia","balvanove pole"), ("vegetacia","huste balvanove pole"), ("vegetacia","kamenity povrch"), ("vegetacia","mociare")]

    find_triples(objects, combs)
    find_foursomes(objects, combs)
    find_fivesomes(objects, combs)
    find_sixsomes(objects, combs)

def find_triples(objects, combs):
    print("Usable triples:\n")
    for obj1 in objects:
        for obj2 in objects:
            for obj3 in objects:
                if((obj1, obj2) in combs and
                        (obj1, obj3) in combs and
                        (obj2, obj3) in combs):
                    print("(" + obj1 + ", " + obj2 + ", " + obj3 + ")")
    print()
    print()



def find_foursomes(objects, combs):
    print("Usable foursomes:\n")
    for obj1 in objects:
        for obj2 in objects:
            for obj3 in objects:
                for obj4 in objects:
                    if((obj1, obj2) in combs and
                            (obj1, obj3) in combs and
                            (obj1, obj4) in combs and
                            (obj2, obj3) in combs and
                            (obj2, obj4) in combs and
                            (obj3, obj4) in combs):
                        print("(" + obj1 + ", " + obj2 + ", " + obj3 + ", " + obj4 +")")
    print()
    print()


def find_fivesomes(objects, combs):
    print("Usable fivesomes:\n")
    for obj1 in objects:
        for obj2 in objects:
            for obj3 in objects:
                for obj4 in objects:
                    for obj5 in objects:
                        if((obj1, obj2) in combs and
                                (obj1, obj3) in combs and
                                (obj1, obj4) in combs and
                                (obj1, obj5) in combs and
                                (obj2, obj3) in combs and
                                (obj2, obj4) in combs and
                                (obj2, obj5) in combs and
                                (obj3, obj4) in combs and
                                (obj3, obj5) in combs and
                                (obj4, obj5) in combs):
                            print("(" + obj1 + ", " + obj2 + ", " + obj3 + ", " + obj4 + ", " + obj5 +")")
    print()
    print()


def find_sixsomes(objects, combs):
    print("Usable sixsomes:\n")
    for obj1 in objects:
        for obj2 in objects:
            for obj3 in objects:
                for obj4 in objects:
                    for obj5 in objects:
                        for obj6 in objects:
                            if((obj1, obj2) in combs and
                                    (obj1, obj3) in combs and
                                    (obj1, obj4) in combs and
                                    (obj1, obj5) in combs and
                                    (obj1, obj6) in combs and
                                    (obj2, obj3) in combs and
                                    (obj2, obj4) in combs and
                                    (obj2, obj5) in combs and
                                    (obj2, obj6) in combs and
                                    (obj3, obj4) in combs and
                                    (obj3, obj5) in combs and
                                    (obj3, obj6) in combs and
                                    (obj4, obj5) in combs and
                                    (obj4, obj6) in combs and
                                    (obj5, obj6) in combs):

                                print("(" + obj1 + ", " + obj2 + ", " + obj3 + ", " + obj4 + ", " + obj5 + ", " + obj6 +")")
    print()
    print()



















if __name__ == '__main__':
    main()

