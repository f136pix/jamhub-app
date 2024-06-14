require "rubygems"
require "bunny"
require "json"

def channel
  # lazy load the channel (only creates new if it doesn't exist yet)
  @channel ||= $rabbitmq_conn.create_channel
end

def async_processor
  @async_processor ||= AsyncProcessor.new
end

# create queue if it doesnt exists yet
que = channel.queue("dotnet.rails", :durable => true, passive: true)

# Set up the consumer to subscribe from the queue
que.subscribe(:block => false) do |delivery_info, properties, payload|
  json_information_message = JSON.parse(payload)
  handle(json_information_message, delivery_info)
end

# invokes service to process the message
# def handle(json_information_message, async_processor = AsyncProcessor.new)
def handle(json_information_message, info)
  routing_key = info[:routing_key]
  async_processor.process(json_information_message, routing_key)
end


