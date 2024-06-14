require 'bunny'

class AsyncPublisher
  class << self
    # DIRECT PUBLISH WITH ROUTING KEY
    def publish(exchange, routing_key, message = {}, queue = 'default')

      channel.confirm_select
      exng = channel.direct("#{exchange}")
      # passive true: doesnt tries to create the queue if exists
      que = channel.queue(queue, passive: true)
      # binds the exchange to te queue
      que.bind(exng, routing_key: routing_key)

      # fanout
      # x = channel.fanout("#{exchange}")
      begin
        json_msg = message.to_json
        exng.publish(json_msg, routing_key: routing_key, mandatory: true)
        puts "--> Sent message #{message} to #{exchange} with routing key #{routing_key}"
      rescue Bunny::Exception => e
        puts "--> There was a error publishing the message: #{e.message}"
      end
    end

    def channel
      # lazy load the channel (only creates new if it doesn't exist yet)
      @channel ||= $rabbitmq_conn.create_channel
    end

  end
end