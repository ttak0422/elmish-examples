namespace NavigationTodos25b.Datas

module Random = 

    let private r = System.Random()

    let next minValue maxValue = r.Next(minValue, maxValue)