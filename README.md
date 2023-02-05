
# RabbitMQDemo
實做/封裝RabbitMQ常見推播模式
* Direct
```
Exchange: demo.test.direct
RouteKey: demo.1
```
* Fanout
```
Exchange: demo.test.fanout
```
* Topic
```
Exchange: demo.test.topic
RouteKey: demo.topic.1
```

## RabbitMQ.Producer
1. TargetFramework: net6.0
2. Reference: Shared.RabbitMQ.Manager
3. 模擬RabbitMQ Server發送推播訊息端
4. `Program.cs`: 注入 `builder.Services.AddSingleton(new RabbitMQManager(RabbitHutch.CreateBus(connectionString));`
5. `ProducerController.cs`: `SendDirect`、`SendFanout`、`SendTopic` 模擬使用不同模式發送訊息

## RabbitMQ.Consumer
1. TargetFramework: net6.0
2. Reference: Shared.RabbitMQ.Manager
3. 模擬RabbitMQ Client接收推播訊息端
4. `Program.cs`: 
    *   DI注入 `builder.Services.AddSingleton(new RabbitMQManager(RabbitHutch.CreateBus(connectionString));`
    *   註冊訂閱Queue `app.MessageQueueSubscribe();` 
5. `MessageQueueService.cs`: 註冊訂閱模式
6. `IMessageConsume.cs`: 繼承此介面並定義接收訊息實作

## Shared.RabbitMQ.Manager
1. TargetFramework: net6.0
2. NuGet: EasyNetQ、EasyNetQ.Serialization.NewtonsoftJson
3. `RabbitMQManager.cs`: 提供RabbitMQ常用功能封裝
    *   推播訊息: PushMessage、PushMessageAsync、SendDirectAsync、SendFanoutAsync、SendTopicAsync
    *   訂閱: Subscribe
4. `IMessageConsume.cs`: 提供Consume接收訊息實作

