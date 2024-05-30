require 'bunny'

class AsyncPublisher
  class << self
    def publish(exchange, routing_key, message = {}, queue = 'jamhub')
      channel.confirm_select
      x = channel.direct("#{exchange}")
      # passive true: doesnt tries to create the queue
      q = channel.queue(queue, passive: true)
      # binds the exchange to te queue
      q.bind(x , routing_key: routing_key)
      begin
        x.publish(message.to_json, routing_key: routing_key, mandatory: true)
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